using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CocktailCreation
{
    public class Shaker : Draggable, IDropHandler
    {
        [SerializeField] private RectTransform sink;
        [SerializeField] private GameObject targetPosition;
        [SerializeField] private RectTransform bin;
        [SerializeField] private float delay;
        [SerializeField] private int numIngredients = 5;
        [SerializeField] private GameObject mixButton;
        
        private readonly List<IngredientType> _ingredientsInShaker = new List<IngredientType>();
        private Vector2 _fillingPosition;

        private void Start()
        {
            _fillingPosition = targetPosition.GetComponent<RectTransform>().anchoredPosition;
        }

        private void Update()
        {
            if (_ingredientsInShaker.Count == numIngredients)
            {
                mixButton.SetActive(true);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            // Check if the dropped object is an ingredient
            if(eventData.pointerDrag!.CompareTag("Ingredient") && !CheckIfIsFull())
            {
                Ingredient ingredient = eventData.pointerDrag.GetComponent<Ingredient>();
                _ingredientsInShaker.Add(ingredient.GetIngredientType());
                
                //DEBUG
                //PrintIngredients(_ingredientsInShaker);
                
            }
        }

        protected override void EndDragBehaviour()
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(sink, Input.mousePosition, Canvas.worldCamera)
                && !CheckIfIsFull())
            {
                _ingredientsInShaker.Add(IngredientType.Water);
                StartCoroutine(ReturnAfterDelay(delay));
            }
            else if (RectTransformUtility.RectangleContainsScreenPoint(bin, Input.mousePosition, Canvas.worldCamera))
            {
                _ingredientsInShaker.Clear();
                ReturnToInitialPosition();
            }
            else
            {
                ReturnToInitialPosition();
            }
        }
        
        private IEnumerator ReturnAfterDelay(float t)
        {
            RectTransform.anchoredPosition = _fillingPosition;
            
            yield return new WaitForSeconds(t);

            ReturnToInitialPosition();
        }
        
        
        public List<IngredientType> GetIngredients()
        {
            return new List<IngredientType>(_ingredientsInShaker);
        }


        public bool CheckIfIsFull()
        {
            if (_ingredientsInShaker.Count <= numIngredients) return false;
            else return true;
        }

        public void EmptyShaker()
        {
            _ingredientsInShaker.Clear();
        }
        
        
        // Debug method to print on console the list of ingredients present in the shaker
        private static void PrintIngredients(List<IngredientType> ingredientList)
        {
            foreach (var t in ingredientList)
            {
                Debug.Log(t);
            }

            Debug.Log("----------------------------------------");
        }
    }
}
