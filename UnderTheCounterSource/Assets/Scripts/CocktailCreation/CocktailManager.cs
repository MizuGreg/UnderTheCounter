using System;
using System.Collections.Generic;
using System.IO;
using CocktailCreation;
using UnityEngine;

namespace CocktailCreation
{
    public class CocktailManager : MonoBehaviour
    {
        [SerializeField] private GameObject ripplePrefab;
        [SerializeField] private RectTransform spawnPoint;
        [SerializeField] private GameObject mixButton;
        [SerializeField] private GameObject cocktailCreationArea;
        [SerializeField] private bool slideInArea;

        
        private readonly Dictionary<IngredientType, int> _ingredientsInShaker = new Dictionary<IngredientType, int>();
        private GameObject _shaker;
        private RectTransform _cocktailCreationAreaRectTransform;
        private Animator _cocktailCreationAreaAnimator;
        

        private void Start()
        {
            _shaker = GameObject.FindGameObjectWithTag("Shaker");
            _cocktailCreationAreaRectTransform = cocktailCreationArea.GetComponent<RectTransform>();
            _cocktailCreationAreaAnimator = cocktailCreationArea.GetComponent<Animator>();
            
        }


        private void Update()
        {
            if (slideInArea) _cocktailCreationAreaAnimator.SetBool("slideIn", true);
            else _cocktailCreationAreaAnimator.SetBool("slideIn", false);
        }


        public void MakeCocktail()
        {
            CreateIngredientsDictionary(_shaker.GetComponent<Shaker>().GetIngredients());
            _shaker.GetComponent<Shaker>().EmptyShaker();
            _shaker.SetActive(false);
            DeactivateMixButton();
            
            //debug
            //PrintIngredientsDictionary();
            
            
            if (_ingredientsInShaker[IngredientType.Verlan] == 2 && _ingredientsInShaker[IngredientType.Shaddock] == 2
                && _ingredientsInShaker[IngredientType.Gryte] == 1)
            {
                GameObject cocktail = Instantiate(ripplePrefab, spawnPoint.position, spawnPoint.rotation, _cocktailCreationAreaRectTransform);
            }
            else
            {
                _shaker.SetActive(true);
            }
            
        }

        private void CreateIngredientsDictionary(List<IngredientType> list)
        {
            ResetIngredientsDictionary(_ingredientsInShaker);
            
            foreach (var ingredient in list)
            {
                if (_ingredientsInShaker.ContainsKey(ingredient))
                {
                    _ingredientsInShaker[ingredient]++;
                }
                else
                {
                    _ingredientsInShaker[ingredient] = 1;
                }
            }
        }

        private void ResetIngredientsDictionary(Dictionary<IngredientType, int> ingredients)
        {
            foreach (IngredientType ingredient in System.Enum.GetValues(typeof(IngredientType)))
            {
                ingredients[ingredient] = 0;
            }
        }
        
        private void PrintIngredientsDictionary()
        {
            Debug.Log("Ingredienti nel dizionario:");

            foreach (var entry in _ingredientsInShaker)
            {
                Debug.Log($"{entry.Key}: {entry.Value}");
            }
        }

        public void ActivateMixButton()
        {
            mixButton.SetActive(true);
        }
        
        public void DeactivateMixButton()
        {
            mixButton.SetActive(false);
        }
        
    }
}
