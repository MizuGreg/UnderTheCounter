using System;
using System.Collections;
using Technical;
using UnityEngine;

namespace CocktailCreation
{
    public class Ingredient : Draggable
    {
        [SerializeField] private IngredientType ingredientType;
        [SerializeField] private RectTransform dropSlot;
        [SerializeField] private float itemRotation;
        [SerializeField] private GameObject targetPosition;
        [SerializeField] private float delay;

        private GameObject _shaker;
        private Vector2 _pouringPosition;

        private void Start()
        {
            _shaker = GameObject.FindGameObjectWithTag("Shaker");
            _pouringPosition = targetPosition.GetComponent<RectTransform>().anchoredPosition;

            EventSystemManager.MakeIngredientInteractable += MakeInteractable;
            EventSystemManager.MakeAllIngredientsInteractable += SetInteractable;

        }

        private void OnDestroy()
        {
            EventSystemManager.MakeIngredientInteractable -= MakeInteractable;
            EventSystemManager.MakeAllIngredientsInteractable -= SetInteractable;
        }

        private void SetInteractable()
        {
            CanvasGroup.blocksRaycasts = true;
        }

        private void MakeInteractable(IngredientType type)
        {
            if (type == ingredientType) CanvasGroup.blocksRaycasts = true;
            else CanvasGroup.blocksRaycasts = false;
        }

        protected override void EndDragBehaviour()
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(dropSlot, Input.mousePosition, Canvas.worldCamera) 
                && _shaker.GetComponent<Shaker>().CanAddIngredient())
            {
                EventSystemManager.OnIngredientPouring();
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
            EventSystemManager.OnIngredientPoured(ingredientType);
        }

        public IngredientType GetIngredientType()
        {
            return ingredientType;
        }
    }
    
}
