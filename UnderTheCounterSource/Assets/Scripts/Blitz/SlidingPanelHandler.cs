using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using Technical;

namespace Blitz
{
    public class SlidingPanelHandler : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        private RectTransform rectTransform;
        private Vector2 originalPosition;

        private void Start()
        {
            EventSystemManager.OnBlitzEnd += ResetPosition;
        }

        private void OnDestroy()
        {
            EventSystemManager.OnBlitzEnd -= ResetPosition;
        }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            originalPosition = rectTransform.anchoredPosition;
        }

        private void ResetPosition()
        {
            rectTransform.anchoredPosition = originalPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            float deltaX = eventData.delta.x;
            float deltaY = eventData.delta.y;

            Vector2 newPosition = rectTransform.anchoredPosition + new Vector2(deltaX, deltaY);
            rectTransform.anchoredPosition = newPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EventSystemManager.OnPanelOpened();
            StartCoroutine(FallOffScreen());
        }

        private IEnumerator FallOffScreen()
        {
            while (rectTransform.anchoredPosition.y > -Screen.height)
            {
                rectTransform.anchoredPosition += Vector2.down * 1500f * Time.deltaTime; 
                yield return null;
            }
        }
    }
}