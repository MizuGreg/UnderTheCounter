using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalPosition;
    private Vector3 originalScale;
    public RectTransform restrictionArea;
    private bool isPlaced = false;

    // Variabile per controllare lo stato delle aree di drop
    private static bool areDropAreasEnabled = false;

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
        StartCoroutine(ShakeAndGrow());
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

        if (IsWithinArea())
        {
            rectTransform.SetParent(restrictionArea, false);
            rectTransform.anchoredPosition = Vector2.zero;
            isPlaced = true;
        }
        else
        {
            rectTransform.anchoredPosition = originalPosition;
        }
    }

    private IEnumerator ShakeAndGrow()
    {
        float duration = 0.2f;
        float shakeMagnitude = 5f;
        float growScale = 1.2f;
        Vector3 targetScale = originalScale * growScale;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float shakeOffsetX = Random.Range(-shakeMagnitude, shakeMagnitude);
            float shakeOffsetY = Random.Range(-shakeMagnitude, shakeMagnitude);
            rectTransform.anchoredPosition += new Vector2(shakeOffsetX, shakeOffsetY);
            rectTransform.localScale = Vector3.Lerp(originalScale, targetScale, t / duration);

            yield return null;
        }

        rectTransform.localScale = targetScale;
    }

    private bool IsWithinArea()
    {
        if (!restrictionArea) return false;

        Vector3[] corners = new Vector3[4];
        restrictionArea.GetWorldCorners(corners);
        Rect areaRect = new Rect(corners[0], corners[2] - corners[0]);

        return areaRect.Contains(rectTransform.position);
    }
}
