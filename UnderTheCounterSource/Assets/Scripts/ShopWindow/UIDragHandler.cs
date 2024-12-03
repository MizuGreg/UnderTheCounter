using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ShopWindow
{
    public class UIDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform _rectTransform;
        private Canvas _canvas;
        private Vector2 _originalPosition;
        private Vector3 _originalScale;
        private Transform _originalParent;
        public List<RectTransform> restrictionAreas = new List<RectTransform>();
        private RectTransform _lastPlacedPlaceholder; 
        private bool _isPlaced = false;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
            _originalScale = _rectTransform.localScale;
            _originalParent = _rectTransform.parent;
            _originalPosition = _rectTransform.anchoredPosition;

            var placeholders = GameObject.FindGameObjectsWithTag("Placeholder");
            foreach (var placeholder in placeholders)
            {
                var placeholderRect = placeholder.GetComponent<RectTransform>();
                if (placeholderRect != null)
                {
                    restrictionAreas.Add(placeholderRect);
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Set the parent to canvas and handle the size/position while dragging
            _rectTransform.SetParent(_canvas.transform, true);
            _rectTransform.localScale = _originalScale;
            _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            _rectTransform.pivot = new Vector2(0.5f, 0.5f);
            _rectTransform.sizeDelta = new Vector2(180, 200);
            _isPlaced = false;

            // Notify PosterPrefabScript that dragging has started
            _rectTransform.GetComponent<PosterPrefabScript>().SetIsDragging(true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var delta = eventData.delta / _canvas.scaleFactor;
            _rectTransform.anchoredPosition += delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var validArea = GetValidRestrictionArea();

            if (validArea != null)
            {
                var dropTarget = validArea.GetComponent<DropTarget>();
                if (dropTarget != null && !dropTarget.IsOccupied())
                {
                    if (_lastPlacedPlaceholder != null && _lastPlacedPlaceholder != validArea)
                    {
                        var lastDropTarget = _lastPlacedPlaceholder.GetComponent<DropTarget>();
                        if (lastDropTarget != null)
                        {
                            lastDropTarget.SetOccupied(false);
                        }
                    }

                    _rectTransform.SetParent(validArea, false);
                    dropTarget.SetOccupied(true);
                    _lastPlacedPlaceholder = validArea;

                    _rectTransform.anchorMin = Vector2.zero;
                    _rectTransform.anchorMax = Vector2.one;
                    _rectTransform.anchoredPosition = Vector2.zero;
                    _rectTransform.sizeDelta = Vector2.zero;
                    _rectTransform.localScale = Vector3.one;

                    _isPlaced = true;
                    _rectTransform.GetComponent<PosterPrefabScript>().TogglePosterDetails(false);
                }
                else
                {
                    ReturnToOriginalPosition();
                }
            }
            else
            {
                ReturnToOriginalPosition();
            }

            // Reset the dragging flag after the drag ends
            _rectTransform.GetComponent<PosterPrefabScript>().SetIsDragging(false);
        }

        private void ReturnToOriginalPosition()
        {
            if (_lastPlacedPlaceholder != null)
            {
                var dropTarget = _lastPlacedPlaceholder.GetComponent<DropTarget>();
                if (dropTarget != null)
                {
                    dropTarget.SetOccupied(false);
                }
            }

            _rectTransform.SetParent(_originalParent, false);
            _rectTransform.anchoredPosition = _originalPosition;
            _rectTransform.sizeDelta = new Vector2(205, 253);
            _rectTransform.localScale = _originalScale;

            PosterPrefabScript posterPrefab = _rectTransform.GetComponent<PosterPrefabScript>();
            if (posterPrefab != null)
            {
                posterPrefab.TogglePosterDetails(true);
            }
        }

        private RectTransform GetValidRestrictionArea()
        {
            foreach (RectTransform restrictionArea in restrictionAreas)
            {
                if (IsWithinArea(restrictionArea))
                {
                    return restrictionArea;
                }
            }
            return null;
        }

        private bool IsWithinArea(RectTransform area)
        {
            Vector3[] corners = new Vector3[4];
            area.GetWorldCorners(corners);
            Rect areaRect = new Rect(corners[0], corners[2] - corners[0]);

            return areaRect.Contains(_rectTransform.position);
        }
    }
}
