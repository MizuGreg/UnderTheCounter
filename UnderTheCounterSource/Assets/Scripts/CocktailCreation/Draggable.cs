using UnityEngine;
using UnityEngine.EventSystems;

namespace CocktailCreation
{
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private bool resetAfterDrop;
        [SerializeField] private bool fixedX;
        [SerializeField] private bool fixedY;
        
        protected Canvas Canvas;
        protected CanvasGroup CanvasGroup;
        protected RectTransform RectTransform;
        private int _originalSiblingIndex;
        private Vector2 _actualPosition;
        private Vector3 _initialPosition;
        private Quaternion _initialRotation;

        protected virtual void Awake()
        {
            Canvas = GetComponentInParent<Canvas>();
            RectTransform = GetComponent<RectTransform>();
            CanvasGroup = GetComponent<CanvasGroup>();
            _originalSiblingIndex = transform.GetSiblingIndex();
            _initialPosition = RectTransform.anchoredPosition;
            _initialRotation = RectTransform.rotation;

            _actualPosition = _initialPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            CanvasGroup.blocksRaycasts = false;
            RectTransform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _actualPosition += eventData.delta / Canvas.scaleFactor;
            if (fixedX)
            {
                _actualPosition.x = _initialPosition.x;
            }
            else if (fixedY)
            {
                _actualPosition.y = _initialPosition.y;
            }
            RectTransform.anchoredPosition = _actualPosition;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            CanvasGroup.blocksRaycasts = true;
            
            EndDragBehaviour();
            
            RectTransform.SetSiblingIndex(_originalSiblingIndex);
            
        }

        protected virtual void EndDragBehaviour()
        {
            if(resetAfterDrop) ReturnToInitialPosition();
        }

        protected void ReturnToInitialPosition()
        {
            _actualPosition = _initialPosition;
            RectTransform.anchoredPosition = _initialPosition;
            RectTransform.rotation = _initialRotation;
        }
    }
}