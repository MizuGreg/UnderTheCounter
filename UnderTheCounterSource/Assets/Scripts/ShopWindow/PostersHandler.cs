using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PostersHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalPosition;
    private Transform originalParent;
    public List<Placeholder> restrictionAreas = new List<Placeholder>();
    public int posterIndex;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        GameObject[] placeholders = GameObject.FindGameObjectsWithTag("Placeholder");
        GameObject[] postersPanel = GameObject.FindGameObjectsWithTag("PostersPanel");
        foreach (GameObject placeholder in placeholders)
        {
            Placeholder placeholderComponent = placeholder.GetComponent<Placeholder>();
            if (placeholderComponent != null)
            {
                restrictionAreas.Add(placeholderComponent);
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        originalParent = rectTransform.parent;
        // Liberare il placeholder precedente
        Placeholder originalPlaceholder = originalParent.GetComponent<Placeholder>();
        if (originalPlaceholder != null)
        {
            originalPlaceholder.isOccupied = false;
        }
        // Set the object to the canvas so that it's on top and not obstructed by other elements.
        rectTransform.SetParent(canvas.transform, true);
        rectTransform.localScale = Vector3.one; // Optionally reset the scale
    }
    public void OnDrag(PointerEventData eventData)
    {
        // Adjust for canvas scale
        Vector2 delta = eventData.delta / canvas.scaleFactor;
        rectTransform.anchoredPosition += delta;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Placeholder validArea = GetValidRestrictionArea();
        if (validArea != null && !validArea.isOccupied)
        {
            // Place the poster in the valid placeholder
            rectTransform.SetParent(validArea.transform, false);
            rectTransform.anchoredPosition = Vector2.zero;
            validArea.isOccupied = true;
        }
        else if (IsMouseOverAreaWithTag("PostersPanel"))
        {
            // Trova tutti gli slot dei poster
            GameObject[] posterSlots = GameObject.FindGameObjectsWithTag("PosterSlot");
            // Trova lo slot corrispondente utilizzando il posterIndex
            foreach (GameObject slot in posterSlots)
            {
                PosterSlot slotComponent = slot.GetComponent<PosterSlot>();
                if (slotComponent != null && slotComponent.posterIndex == posterIndex)
                {
                    // Imposta il genitore del poster su quello slot
                    rectTransform.SetParent(slot.transform, false);
                    rectTransform.anchoredPosition = Vector2.zero;
                    // Segna lo slot come occupato
                    slotComponent.isOccupied = true;
                    break;
                }
            }
        }
        else
        {
            // Return to original parent and position on invalid drop
            rectTransform.SetParent(originalParent, true);
            rectTransform.anchoredPosition = originalPosition;
            // Re-occupy the original placeholder
            Placeholder originalPlaceholder = originalParent.GetComponent<Placeholder>();
            if (originalPlaceholder != null)
            {
                originalPlaceholder.isOccupied = true;
            }
        }
    }
    private Placeholder GetValidRestrictionArea()
    {
        foreach (Placeholder restrictionArea in restrictionAreas)
        {
            if (IsWithinArea(restrictionArea.GetComponent<RectTransform>()))
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
        Vector2 mousePosition = Input.mousePosition;
        return mousePosition.x >= corners[0].x && mousePosition.x <= corners[2].x &&
               mousePosition.y >= corners[0].y && mousePosition.y <= corners[2].y;
    }
    private bool IsMouseOverAreaWithTag(string tag)
    {
        GameObject[] areas = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject area in areas)
        {
            RectTransform areaRect = area.GetComponent<RectTransform>();
            if (IsWithinArea(areaRect))
            {
                return true;
            }
        }
        return false;
    }
}