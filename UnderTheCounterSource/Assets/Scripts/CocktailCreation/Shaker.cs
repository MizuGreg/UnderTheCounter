using System.Collections.Generic;
using Technical;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CocktailCreation
{
    public class Shaker : MonoBehaviour, IDropHandler
    {
        [SerializeField] private int numIngredients = 5;
        
        private readonly List<IngredientType> _ingredientsInShaker = new List<IngredientType>();
        

        public void OnDrop(PointerEventData eventData)
        {
            // Check if the dropped object is an ingredient
            if(eventData.pointerDrag!.CompareTag("Ingredient") && !CheckIfIsFull())
            {
                Ingredient ingredient = eventData.pointerDrag.GetComponent<Ingredient>();
                _ingredientsInShaker.Add(ingredient.GetIngredientType());
                
                EventSystemManager.OnIngredientAdded(_ingredientsInShaker.Count, ingredient.GetIngredientType());
                
                if (_ingredientsInShaker.Count == numIngredients)
                {
                    EventSystemManager.OnShakerFull();
                }
                
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
            if (_ingredientsInShaker.Count < numIngredients) return false;
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
