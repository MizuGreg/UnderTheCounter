using System;
using System.Collections;
using Technical;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CocktailCreation
{
    public class GarnishDisplay : Draggable
    {
        public Garnish garnish;

        private Vector2 _pointerDelta;
        private Vector2 _dropSlotPosition = new Vector2(0,0);
        private bool _isInPosition;
        private bool _dropSlotSet;

        private void Start()
        {
            this.GetComponent<Image>().sprite = garnish.sprite;
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.0039f;
        }
        
        
        public override void OnDrag(PointerEventData eventData)
        {
            // Calculate the vertical delta movement, ignoring the horizontal delta
            _pointerDelta = new Vector2(0, eventData.delta.y / Canvas.scaleFactor); // Only vertical movement
            
            // Limit the drag speed to prevent large jumps
            _pointerDelta.y = Mathf.Clamp(_pointerDelta.y, -30f, 30f); // Adjust the limits as needed
            
            // Apply the vertical delta to the current position
            ActualPosition.y += _pointerDelta.y;

            // Clamp the position to enforce vertical boundaries
            ActualPosition.y = Mathf.Clamp(ActualPosition.y, _dropSlotPosition.y, InitialPosition.y);

            // Apply the clamped position (horizontal position remains unchanged)
            RectTransform.anchoredPosition = new Vector2(RectTransform.anchoredPosition.x, ActualPosition.y);
        }


        

        protected override void EndDragBehaviour()
        {
            if (_isInPosition)
            {
                EventSystemManager.OnGarnishAdded();
            }
            else
            {
                StartCoroutine(SmoothReturnToInitialPosition());
            }
        }
        
        private IEnumerator SmoothReturnToInitialPosition()
        {
            float duration = 0.3f;
            float elapsed = 0f;
            
            CanvasGroup.blocksRaycasts = false;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                t = Mathf.SmoothStep(0, 1, t);
                RectTransform.anchoredPosition = Vector2.Lerp(ActualPosition, InitialPosition, t);
                yield return null;
            }

            ActualPosition = InitialPosition;
            RectTransform.anchoredPosition = ActualPosition;
            
            CanvasGroup.blocksRaycasts = true;
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Cocktail"))
            {
                _isInPosition = true;
                if (_dropSlotPosition.y == 0)
                {
                    _dropSlotPosition.y = ActualPosition.y;
                }
            }
        }
        
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Cocktail"))
            {
                _isInPosition = false;
            }
        }
    }
}
