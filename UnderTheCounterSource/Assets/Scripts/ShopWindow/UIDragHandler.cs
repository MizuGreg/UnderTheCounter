using System.Collections.Generic;
using System.Linq;
using Technical;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace ShopWindow
{
    public class UIDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private PosterPrefabScript _pps;
        private RectTransform _rectTransform;
        private Canvas _canvas;
        private Vector2 _originalPosition;
        private Vector3 _originalScale;
        private Transform _originalParent;
        public List<RectTransform> restrictionAreas;
        [SerializeField] private RectTransform lastPlacedPlaceholder; 

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _pps = _rectTransform.GetComponent<PosterPrefabScript>();
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

            // If I forgot to set the starting slot in which the poster is in, this automatically sets it to the parent
            // _lastPlacedPlaceholder ??= gameObject.GetComponentInParent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Prevent drag if the poster is locked
            if (!IsDraggable()) return;
            
            // Set the parent to canvas and handle the size/position while dragging
            _rectTransform.SetParent(_canvas.transform, true);
            _rectTransform.localScale = _originalScale;
            _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            _rectTransform.pivot = new Vector2(0.5f, 0.5f);
            _rectTransform.sizeDelta = new Vector2(180, 200);

            // Notify PosterPrefabScript that dragging has started
            _pps.SetIsDragging(true);
            
            // Hide price/"owned" underneath poster
            _pps.TogglePosterDetails(false);
            
            // Play sound, but only if we're ripping the poster from the window
            if (lastPlacedPlaceholder != null)
            {
                EventSystemManager.OnPosterRippedDown();
            }
        }

        private bool IsDraggable()
        {
            return _pps != null && !_pps.isLocked && _pps.isVisible;
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Prevent drag if the poster is locked
            if (!IsDraggable()) return;
            
            var delta = eventData.delta / _canvas.scaleFactor;
            _rectTransform.anchoredPosition += delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Prevent drag if the poster is locked
            if (!IsDraggable()) return;
            
            var validArea = GetValidRestrictionArea();
            
            if (validArea != null)
            {
                var dropTarget = validArea.GetComponent<DropTarget>();
                if (dropTarget != null && !dropTarget.IsOccupied())
                {
                    if (lastPlacedPlaceholder != null && lastPlacedPlaceholder != validArea)
                    {
                        var lastDropTarget = lastPlacedPlaceholder.GetComponent<DropTarget>();
                        if (lastDropTarget != null)
                        {
                            lastDropTarget.SetOccupied(false);
                        }
                    }

                    // ACTUAL HANGING PART
                    HangPoster(validArea, dropTarget, _pps);
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

        public void HangPoster(RectTransform validArea, DropTarget dropTarget, PosterPrefabScript posterPrefab)
        {
            EventSystemManager.OnPosterHung(); // plays sound when a poster is hung on the window
            _rectTransform.SetParent(validArea, false);
            dropTarget.SetOccupied(true);
            lastPlacedPlaceholder = validArea;

            _rectTransform.anchorMin = Vector2.zero;
            _rectTransform.anchorMax = Vector2.one;
            _rectTransform.anchoredPosition = Vector2.zero;
            _rectTransform.sizeDelta = Vector2.zero;
            _rectTransform.localScale = Vector3.one;
            posterPrefab.hanged = dropTarget.ID;
        }

        private void ReturnToOriginalPosition()
        {
            if (lastPlacedPlaceholder != null)
            {
                var dropTarget = lastPlacedPlaceholder.GetComponent<DropTarget>();
                if (dropTarget != null)
                {
                    dropTarget.SetOccupied(false);
                    // EventSystemManager.OnPosterRippedDown();
                    lastPlacedPlaceholder = null;
                }
            }
            
            _rectTransform.SetParent(_originalParent, false);
            _rectTransform.anchoredPosition = _originalPosition;
            _rectTransform.sizeDelta = new Vector2(205, 253);
            _rectTransform.localScale = _originalScale;

            var posterPrefab = _rectTransform.GetComponent<PosterPrefabScript>();
            if (posterPrefab == null) return;
            posterPrefab.hanged = 0;

            posterPrefab.TogglePosterDetails(true);
        }

        private RectTransform GetValidRestrictionArea()
        {
            return restrictionAreas.FirstOrDefault(IsWithinArea);
        }

        private bool IsWithinArea(RectTransform area)
        {
            var corners = new Vector3[4];
            area.GetWorldCorners(corners);
            var areaRect = new Rect(corners[0], corners[2] - corners[0]);

            return areaRect.Contains(_rectTransform.position);
        }
    }
}
