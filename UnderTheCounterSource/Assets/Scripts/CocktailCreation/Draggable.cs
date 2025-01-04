using UnityEngine;
using UnityEngine.EventSystems;

namespace CocktailCreation
{
    public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private bool resetAfterDrop;
        
        protected Canvas Canvas;
        protected CanvasGroup CanvasGroup;
        protected RectTransform RectTransform;
        private int _originalSiblingIndex;
        protected Vector2 ActualPosition;
        protected Vector3 InitialPosition;
        private Quaternion _initialRotation;

        protected virtual void Awake()
        {
            Canvas = GetComponentInParent<Canvas>();
            RectTransform = GetComponent<RectTransform>();
            CanvasGroup = GetComponent<CanvasGroup>();
            _originalSiblingIndex = transform.GetSiblingIndex();
            InitialPosition = RectTransform.anchoredPosition;
            _initialRotation = RectTransform.rotation;

            ActualPosition = InitialPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            CanvasGroup.blocksRaycasts = false;
            RectTransform.SetAsLastSibling();
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            RectTransform.anchoredPosition += eventData.delta / Canvas.scaleFactor;
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
            ActualPosition = InitialPosition;
            RectTransform.anchoredPosition = InitialPosition;
            RectTransform.rotation = _initialRotation;
        }
    }
}