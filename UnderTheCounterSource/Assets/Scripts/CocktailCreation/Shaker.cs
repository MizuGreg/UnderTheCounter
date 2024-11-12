using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CocktailCreation
{
    public class Shaker : MonoBehaviour, IDropHandler
    {
        [SerializeField] private int numIngredients = 5;
        [SerializeField] private GameObject mixButton;
        
        private readonly List<IngredientType> _ingredientsInShaker = new List<IngredientType>();
        

        private void Update()
        {
            // todo: is not necessary do this every frame
            if (_ingredientsInShaker.Count == numIngredients)
            {
                mixButton.SetActive(true);
            }
            else
            {
                mixButton.SetActive(false);
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
        private void PrintIngredients(List<IngredientType> ingredientList)
        {
            foreach (var t in ingredientList)
            {
                Debug.Log(t);
            }

            Debug.Log("----------------------------------------");
        }
    }
}
