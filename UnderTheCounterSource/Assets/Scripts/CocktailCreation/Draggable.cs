using UnityEngine;
using UnityEngine.EventSystems;

namespace CocktailCreation
{
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] protected Canvas canvas;

        protected CanvasGroup CanvasGroup;
        protected RectTransform RectTransform;
        private Vector3 _initialPosition;
        private Quaternion _initialRotation;

        protected virtual void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            CanvasGroup = GetComponent<CanvasGroup>();
            _initialPosition = RectTransform.anchoredPosition;
            _initialRotation = RectTransform.rotation;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            CanvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            CanvasGroup.blocksRaycasts = true;
        }

        protected void ReturnToInitialPosition()
        {
            RectTransform.anchoredPosition = _initialPosition;
            RectTransform.rotation = _initialRotation;
        }
    }
}