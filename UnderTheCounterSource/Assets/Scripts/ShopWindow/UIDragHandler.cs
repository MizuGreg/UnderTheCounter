using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace ShopWindow
{
    public class UIDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform _rectTransform;
        private Canvas _canvas;
        private Vector2 _originalPosition;
        private Vector3 _originalScale;
        private Transform _originalParent;
        public List<RectTransform> restrictionAreas;
        [FormerlySerializedAs("_lastPlacedPlaceholder")] 
        [SerializeField] private RectTransform lastPlacedPlaceholder; 

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

            // If I forgot to set the starting slot in which the poster is in, this automatically sets it to the parent
            // _lastPlacedPlaceholder ??= gameObject.GetComponentInParent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            // Prevent drag if the poster is locked
            var posterPrefabScript = _rectTransform.GetComponent<PosterPrefabScript>();
            if (posterPrefabScript != null && posterPrefabScript.isLocked)
            {
                return; // Exit early if the poster is locked
            }
            print("drag begin!");
            // Set the parent to canvas and handle the size/position while dragging
            _rectTransform.SetParent(_canvas.transform, true);
            _rectTransform.localScale = _originalScale;
            _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            _rectTransform.pivot = new Vector2(0.5f, 0.5f);
            _rectTransform.sizeDelta = new Vector2(180, 200);

            // Notify PosterPrefabScript that dragging has started
            posterPrefabScript.SetIsDragging(true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Prevent drag if the poster is locked
            var posterPrefabScript = _rectTransform.GetComponent<PosterPrefabScript>();
            if (posterPrefabScript != null && posterPrefabScript.isLocked)
            {
                Debug.Log("exiting OnDrag");
                return; // Exit early if the poster is locked
            }
            var delta = eventData.delta / _canvas.scaleFactor;
            _rectTransform.anchoredPosition += delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Prevent drag if the poster is locked
            var posterPrefab = _rectTransform.GetComponent<PosterPrefabScript>();
            
            if (posterPrefab != null && posterPrefab.isLocked)
            {
                Debug.Log("exiting OnEndDrag");
                return; // Exit early if the poster is locked
            }
            
            posterPrefab.TogglePosterDetails(false);
            
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

                    _rectTransform.SetParent(validArea, false);
                    dropTarget.SetOccupied(true);
                    lastPlacedPlaceholder = validArea;

                    _rectTransform.anchorMin = Vector2.zero;
                    _rectTransform.anchorMax = Vector2.one;
                    _rectTransform.anchoredPosition = Vector2.zero;
                    _rectTransform.sizeDelta = Vector2.zero;
                    _rectTransform.localScale = Vector3.one;
                    
                    posterPrefab.AddPosterToHungPosters();
                    print("added poster to hung ones");
                }
                else
                {
                    ReturnToOriginalPosition();
                    print("Return to original position!");
                }
            }
            else
            {
                ReturnToOriginalPosition();
                print("Return to original position!");
            }

            // Reset the dragging flag after the drag ends
            _rectTransform.GetComponent<PosterPrefabScript>().SetIsDragging(false);
        }

        private void ReturnToOriginalPosition()
        {
            if (lastPlacedPlaceholder != null)
            {
                var dropTarget = lastPlacedPlaceholder.GetComponent<DropTarget>();
                if (dropTarget != null)
                {
                    dropTarget.SetOccupied(false);
                }
            }

            _rectTransform.SetParent(_originalParent, false);
            _rectTransform.anchoredPosition = _originalPosition;
            _rectTransform.sizeDelta = new Vector2(205, 253);
            _rectTransform.localScale = _originalScale;

            var posterPrefab = _rectTransform.GetComponent<PosterPrefabScript>();
            if (posterPrefab == null) return;
            posterPrefab.RemovePosterFromHungPosters();
            print("removed poster from hung ones");
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
