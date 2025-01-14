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
        private bool _dropSlotSet;

        private float _tolerance = 5f;

        private void Start()
        {
            this.GetComponent<Image>().sprite = garnish.sprite;
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.0039f;

            _dropSlotPosition.y = GameObject.FindGameObjectWithTag("GarnishDropPosition").GetComponent<RectTransform>()
                .anchoredPosition.y;
        }
        
        
        public override void OnDrag(PointerEventData eventData)
        {
            // Calculate the vertical delta movement, ignoring the horizontal delta
            _pointerDelta = new Vector2(0, eventData.delta.y / Canvas.scaleFactor); // Only vertical movement
            
            // Limit the drag speed to prevent large jumps
            _pointerDelta.y = Mathf.Clamp(_pointerDelta.y, -80f, 80f); // Increase limits for faster drag

            // Check if the mouse is moving up or down
            if ((_pointerDelta.y > 0 && ActualPosition.y < InitialPosition.y) || (_pointerDelta.y < 0 && ActualPosition.y > _dropSlotPosition.y))
            {
                // Apply the vertical delta to the current position only if the movement is within valid bounds
                ActualPosition.y += _pointerDelta.y;

                // Clamp the position to enforce vertical boundaries
                ActualPosition.y = Mathf.Clamp(ActualPosition.y, _dropSlotPosition.y, InitialPosition.y);

                // Apply the clamped position (horizontal position remains unchanged)
                RectTransform.anchoredPosition = new Vector2(RectTransform.anchoredPosition.x, ActualPosition.y);
            }
        }

        protected override void EndDragBehaviour()
        {
            if (RectTransform.anchoredPosition.y <= _dropSlotPosition.y + _tolerance)
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
        
    }
}
