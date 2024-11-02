using UnityEngine;
using UnityEngine.EventSystems;

namespace CocktailCreation
{
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private bool resetAfterDrop;
        
        protected Canvas Canvas;
        private CanvasGroup _canvasGroup;
        protected RectTransform RectTransform;
        private int _originalSiblingIndex;
        private Vector3 _initialPosition;
        private Quaternion _initialRotation;

        protected virtual void Awake()
        {
            Canvas = GetComponentInParent<Canvas>();
            RectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _originalSiblingIndex = transform.GetSiblingIndex();
            _initialPosition = RectTransform.anchoredPosition;
            _initialRotation = RectTransform.rotation;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;
            RectTransform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransform.anchoredPosition += eventData.delta / Canvas.scaleFactor;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            
            EndDragBehaviour();
            
            RectTransform.SetSiblingIndex(_originalSiblingIndex);
            
        }

        protected virtual void EndDragBehaviour()
        {
            if(resetAfterDrop) ReturnToInitialPosition();
        }

        protected void ReturnToInitialPosition()
        {
            RectTransform.anchoredPosition = _initialPosition;
            RectTransform.rotation = _initialRotation;
        }
    }
}