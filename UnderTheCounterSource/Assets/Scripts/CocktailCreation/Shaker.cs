using System;
using System.Collections.Generic;
using Technical;
using UnityEngine;

namespace CocktailCreation
{
    public class Shaker : MonoBehaviour
    {
        [SerializeField] private int numIngredients = 5;
        
        private readonly List<IngredientType> _ingredientsInShaker = new List<IngredientType>();

        private bool _isBusy;


        private void Start()
        {
            // Subscribe to events
            EventSystemManager.OnIngredientPouring += SetShakerBusy;
            EventSystemManager.OnIngredientPoured += AddIngredient;
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            EventSystemManager.OnIngredientPouring -= SetShakerBusy;
            EventSystemManager.OnIngredientPoured -= AddIngredient;
        }

        private void AddIngredient(IngredientType ingredient)
        {
            // Add the ingredient in the shaker and update the UI Ingredients Bar
            _ingredientsInShaker.Add(ingredient);
            EventSystemManager.OnIngredientAdded(_ingredientsInShaker.Count, ingredient);
            
            SetShakerNotBusy();
                
            // If the shaker is full, tell it to the CocktailManager
            if (_ingredientsInShaker.Count == numIngredients)
            {
                EventSystemManager.OnShakerFull();
            }
            
            //DEBUG
            //PrintIngredients(_ingredientsInShaker);
        }

        private void SetShakerBusy()
        {
            this._isBusy = true;
        }
        
        private void SetShakerNotBusy()
        {
            this._isBusy = false;
        }
        
        
        public List<IngredientType> GetIngredients()
        {
            return new List<IngredientType>(_ingredientsInShaker);
        }


         private bool IsFull()
        {
            if (_ingredientsInShaker.Count < numIngredients) return false;
            else return true;
        }

        public bool CanAddIngredient()
        {
            if (!IsFull() && !_isBusy) return true;
            else return false;
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
