using UnityEngine;
using UnityEngine.EventSystems;

public class SlidingPanelHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    public Vector2 openOffset = new Vector2(300f, 0f);
    public float slideThresholdOpen = 100f;
    public float slideThresholdClose;
    public float slideSpeed = 500f;
    private bool isOpen = false;

    private float closedLimitX;
    private float openLimitX;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;

        closedLimitX = originalPosition.x;
        openLimitX = originalPosition.x + openOffset.x;

        slideThresholdClose = openOffset.x - slideThresholdOpen;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float deltaX = eventData.delta.x;

        float newX = rectTransform.anchoredPosition.x + deltaX;

        if (!isOpen && newX < closedLimitX)
        {
            return;
        }

        if (isOpen && newX > openLimitX)
        {
            return;
        }

        newX = Mathf.Clamp(newX, closedLimitX, openLimitX);

        rectTransform.anchoredPosition = new Vector2(newX, rectTransform.anchoredPosition.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float draggedDistance = rectTransform.anchoredPosition.x - originalPosition.x;

        if (!isOpen && draggedDistance >= slideThresholdOpen)
        {
            SlidePanelTo(originalPosition + openOffset);
            isOpen = true;
            UIDragHandler.EnableDropAreas();
        }
        else if (isOpen && draggedDistance <= slideThresholdClose)
        {
            SlidePanelTo(originalPosition);
            isOpen = false;
            UIDragHandler.DisableDropAreas();
        }
        else
        {
            SlidePanelTo(isOpen ? originalPosition + openOffset : originalPosition);
        }
    }

    private void SlidePanelTo(Vector2 targetPosition)
    {
        StopAllCoroutines();
        StartCoroutine(SlideToPosition(targetPosition));
    }

    private System.Collections.IEnumerator SlideToPosition(Vector2 targetPosition)
    {
        while (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) > 1f)
        {
            rectTransform.anchoredPosition = Vector2.MoveTowards(
                rectTransform.anchoredPosition,
                targetPosition,
                slideSpeed * Time.deltaTime
            );
            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
    }
}
