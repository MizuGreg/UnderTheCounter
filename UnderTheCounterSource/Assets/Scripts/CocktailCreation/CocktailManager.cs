using System;
using System.Collections.Generic;
using System.IO;
using CocktailCreation;
using UnityEngine;

namespace CocktailCreation
{
    public class CocktailManager : MonoBehaviour
    {
        [SerializeField] private RectTransform spawnPoint;
        [SerializeField] private GameObject mixButton;
        [SerializeField] private GameObject cocktailCreationArea;
        [SerializeField] private bool slideInArea;

        
        private readonly Dictionary<IngredientType, int> _ingredientsInShaker = new Dictionary<IngredientType, int>();
        private GameObject _shaker;
        private RectTransform _cocktailCreationAreaRectTransform;
        private Animator _cocktailCreationAreaAnimator;

        private GameObject _ripplePrefab;
        private GameObject _everestPrefab;
        private GameObject _springBeePrefab;
        private GameObject _partiPrefab;
        private GameObject _magazinePrefab;

        private void Start()
        {
            _shaker = GameObject.FindGameObjectWithTag("Shaker");
            _cocktailCreationAreaRectTransform = cocktailCreationArea.GetComponent<RectTransform>();
            _cocktailCreationAreaAnimator = cocktailCreationArea.GetComponent<Animator>();
            
            // Carica il prefab dalla cartella Resources
            _ripplePrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/Ripple");
            _everestPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/Everest");
            _springBeePrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/SpringBee");
            _partiPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/Parti");
            _magazinePrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/Magazine");
            
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
            
            // todo: use a json file instead of hardcoding recipes here
            if (_ingredientsInShaker[IngredientType.Verlan] == 2 && _ingredientsInShaker[IngredientType.Shaddock] == 2
                && _ingredientsInShaker[IngredientType.Gryte] == 1)
            {
                GameObject cocktail = Instantiate(_ripplePrefab, spawnPoint.position, spawnPoint.rotation, _cocktailCreationAreaRectTransform);
            }
            else if (_ingredientsInShaker[IngredientType.Caledon] == 3 && _ingredientsInShaker[IngredientType.Gryte] == 2)
            {
                GameObject cocktail = Instantiate(_everestPrefab, spawnPoint.position, spawnPoint.rotation, _cocktailCreationAreaRectTransform);
            }
            else if (_ingredientsInShaker[IngredientType.Verlan] == 3 && _ingredientsInShaker[IngredientType.Shaddock] == 2)
            {
                GameObject cocktail = Instantiate(_springBeePrefab, spawnPoint.position, spawnPoint.rotation, _cocktailCreationAreaRectTransform);
            }
            else if (_ingredientsInShaker[IngredientType.Verlan] == 2 && _ingredientsInShaker[IngredientType.Ferrucci] == 3)
            {
                GameObject cocktail = Instantiate(_partiPrefab, spawnPoint.position, spawnPoint.rotation, _cocktailCreationAreaRectTransform);
            }
            else if (_ingredientsInShaker[IngredientType.Verlan] == 3 && _ingredientsInShaker[IngredientType.Ferrucci] == 1
                     && _ingredientsInShaker[IngredientType.Shaddock] == 1)
            {
                GameObject cocktail = Instantiate(_magazinePrefab, spawnPoint.position, spawnPoint.rotation, _cocktailCreationAreaRectTransform);
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
