using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CocktailCreation
{
    public class Ingredient : Draggable
    {
        [SerializeField] private IngredientType ingredientType;
        [SerializeField] private RectTransform dropSlot;
        [SerializeField] private float itemRotation;
        [SerializeField] private GameObject targetPosition;
        [SerializeField] private float delay;

        private Vector2 _pouringPosition;

        private void Start()
        {
            _pouringPosition = targetPosition.GetComponent<RectTransform>().anchoredPosition;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            CanvasGroup.blocksRaycasts = true;
            
            if (RectTransformUtility.RectangleContainsScreenPoint(dropSlot, Input.mousePosition, canvas.worldCamera))
            {
                StartCoroutine(ReturnAfterDelay(delay));
            }
            else
            {
                ReturnToInitialPosition();
            }
        }

        private IEnumerator ReturnAfterDelay(float t)
        {
            RectTransform.rotation = Quaternion.Euler(0, 0, itemRotation);
            RectTransform.anchoredPosition = _pouringPosition; 

            yield return new WaitForSeconds(t);

            ReturnToInitialPosition();
        }

        public IngredientType GetIngredientType()
        {
            return ingredientType;
        }
    }
    
}
