using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Technical;

namespace Blitz
{
    public class UIDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform rectTransform;
        private Canvas canvas;
        private Vector2 originalPosition;
        private Vector3 originalScale;
        private bool isPlaced = false;
        private static bool areDropAreasEnabled = true;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            originalScale = rectTransform.localScale;
        }

        public static void EnableDropAreas()
        {
            areDropAreasEnabled = true;
        }

        public static void DisableDropAreas()
        {
            areDropAreasEnabled = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!areDropAreasEnabled || isPlaced) return;

            originalPosition = rectTransform.anchoredPosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!areDropAreasEnabled || isPlaced) return;

            Vector2 delta = eventData.delta / canvas.scaleFactor;
            rectTransform.anchoredPosition += delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!areDropAreasEnabled || isPlaced) return;

            rectTransform.localScale = originalScale;

            if (string.IsNullOrEmpty(gameObject.tag) || gameObject.tag == "Untagged")
            {
                rectTransform.anchoredPosition = originalPosition;
                return;
            }

            if (IsWithinArea())
            {
                rectTransform.SetParent(GetMatchingRestrictionArea(), false);
                rectTransform.anchoredPosition = Vector2.zero;
                isPlaced = true;
                EventSystemManager.OnBottlePlaced();
            }
            else
            {
                rectTransform.anchoredPosition = originalPosition;
            }
        }

        private bool IsWithinArea()
        {
            RectTransform restrictionArea = GetMatchingRestrictionArea();
            if (!restrictionArea) return false;

            Vector3[] corners = new Vector3[4];
            restrictionArea.GetWorldCorners(corners);
            Rect areaRect = new Rect(corners[0], corners[2] - corners[0]);

            return areaRect.Contains(rectTransform.position);
        }

        private RectTransform GetMatchingRestrictionArea()
        {
            GameObject area = GameObject.FindGameObjectWithTag(gameObject.tag);
            RectTransform rect = area.GetComponent<RectTransform>();
            if (rect != null)
            {
                return rect;
            }
            return null;
        }

    }
}