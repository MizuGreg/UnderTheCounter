using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

namespace Blitz
{
    public class SlidingPanelHandler : MonoBehaviour, IDragHandler, IEndDragHandler
    {
        private RectTransform rectTransform;
        private Vector2 originalPosition;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            originalPosition = rectTransform.anchoredPosition;
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
            StartCoroutine(FallOffScreen());
        }

        private IEnumerator FallOffScreen()
        {
            while (rectTransform.anchoredPosition.y > -Screen.height * 2f)
            {
                rectTransform.anchoredPosition += Vector2.down * 1500f * Time.deltaTime; 
                yield return null;
            }
        }
    }
}