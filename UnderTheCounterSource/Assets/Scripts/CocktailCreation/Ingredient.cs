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
        [SerializeField] private float delay;
        
        private GameObject _shaker;

        private void Start()
        {
            _shaker = GameObject.FindGameObjectWithTag("Shaker");

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

        protected override void CustomEffect()
        {
            // While dragging over the shaker it changes the rotation

            if (RectTransformUtility.RectangleContainsScreenPoint(dropSlot, Input.mousePosition, Canvas.worldCamera)
                && _shaker.GetComponent<Shaker>().CanAddIngredient())
            {
                if (Input.mousePosition.x >= dropSlot.position.x)
                {
                    RectTransform.rotation = Quaternion.Euler(0, 0, itemRotation);
                }
                else
                {
                    RectTransform.rotation = Quaternion.Euler(0, 0, -itemRotation);
                }
            }
            else
            {
                RectTransform.rotation = Quaternion.Euler(0, 0, 0);
            }
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
            if (RectTransform.rotation.z >= 0)
            {
                RectTransform.anchoredPosition = new Vector2(-392, -196);
            }
            else
            {
                RectTransform.anchoredPosition = new Vector2(-574, -196);
            }

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
