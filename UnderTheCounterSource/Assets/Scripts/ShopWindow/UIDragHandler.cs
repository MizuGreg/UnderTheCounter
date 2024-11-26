using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalPosition;
    private Vector3 originalScale;
    private Transform originalParent;
    public List<RectTransform> restrictionAreas = new List<RectTransform>();
    private bool isPlaced = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalScale = rectTransform.localScale;

        // Find all objects tagged as "Placeholder" and add their RectTransforms to restrictionAreas
        GameObject[] placeholders = GameObject.FindGameObjectsWithTag("Placeholder");
        foreach (GameObject placeholder in placeholders)
        {
            RectTransform placeholderRect = placeholder.GetComponent<RectTransform>();
            if (placeholderRect != null)
            {
                restrictionAreas.Add(placeholderRect);
            }
        }

        if (restrictionAreas.Count == 0)
        {
            Debug.LogWarning("No restriction areas found with the tag 'Placeholder'.");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Save the original position and parent and size
        originalPosition = rectTransform.anchoredPosition;
        originalParent = rectTransform.parent;
        // originalScale = rectTransform.sizeDelta;

        // Reset scale and size to the original state while dragging
        rectTransform.localScale = originalScale;
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f); // Center anchors
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f); // Center anchors
        rectTransform.pivot = new Vector2(0.5f, 0.5f);     // Center pivot
        rectTransform.sizeDelta = new Vector2(180, 200);   // Replace with original size

        // Temporarily move to the root canvas to render on top
        rectTransform.SetParent(canvas.transform, true);

        isPlaced = false; // Reset placement status
    }


    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.delta / canvas.scaleFactor;
        rectTransform.anchoredPosition += delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RectTransform validArea = GetValidRestrictionArea();
        if (validArea != null)
        {
            // Place the poster in the valid placeholder
            rectTransform.SetParent(validArea, false);

            // Match the size and position of the placeholder
            rectTransform.anchorMin = Vector2.zero; // Stretch to fill
            rectTransform.anchorMax = Vector2.one;  // Stretch to fill
            rectTransform.anchoredPosition = Vector2.zero; // Center in placeholder
            rectTransform.sizeDelta = Vector2.zero; // Reset size adjustments
            rectTransform.localScale = Vector3.one; // Ensure uniform scaling
            
            isPlaced = true;
            
            rectTransform.GetComponent<PosterPrefabScript>().TogglePosterDetails(!isPlaced);
        }
        else
        {
            // Return to original parent and position on invalid drop
            rectTransform.SetParent(originalParent, true);
            rectTransform.anchoredPosition = originalPosition;
            if (originalParent.CompareTag("Placeholder"))
                rectTransform.sizeDelta = originalParent.GetComponent<RectTransform>().sizeDelta;
            else
                rectTransform.sizeDelta = new Vector2(205, 253);
            // Do NOT reset the size or scale here to retain the current size
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

        return areaRect.Contains(rectTransform.position);
    }
}