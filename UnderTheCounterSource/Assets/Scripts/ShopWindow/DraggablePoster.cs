using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePoster : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas _parentCanvas; // Reference to the canvas the object is on
    private RectTransform _rectTransform; // RectTransform of the draggable object
    private CanvasGroup _canvasGroup; // CanvasGroup to manage drag transparency

    private Vector2 _originalPosition; // Store the original position in case the drag is canceled

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _parentCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Save the original position
        _originalPosition = _rectTransform.anchoredPosition;

        // Make the poster semi-transparent while dragging
        _canvasGroup.alpha = 0.6f;
        _canvasGroup.blocksRaycasts = false; // Allow raycasts to pass through this object
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the object with the pointer
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentCanvas.transform as RectTransform,
            eventData.position,
            _parentCanvas.worldCamera,
            out Vector2 localPoint);

        _rectTransform.anchoredPosition = localPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restore the transparency and raycast blocking
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;

        // If not dropped on a valid placeholder, return to the original position
        if (eventData.pointerEnter == null || !eventData.pointerEnter.CompareTag("Placeholder"))
        {
            _rectTransform.anchoredPosition = _originalPosition;
        }
    }
}