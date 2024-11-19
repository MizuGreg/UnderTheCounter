using UnityEngine;
using UnityEngine.EventSystems;

public class DropTarget : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // Check if the dragged object is a poster
        var draggedObject = eventData.pointerDrag;
        if (draggedObject != null && draggedObject.GetComponent<DraggablePoster>() != null)
        {
            // Move the poster to this placeholder
            RectTransform draggedRect = draggedObject.GetComponent<RectTransform>();
            draggedRect.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }
    }
}