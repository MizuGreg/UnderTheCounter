using System;
using System.Collections.Generic;
using Technical;
using UnityEngine;

namespace CocktailCreation
{
    public class CocktailManager : MonoBehaviour
    {
        [SerializeField] private RectTransform spawnPoint;
        [SerializeField] private RectTransform garnishSpawnPoint;
        [SerializeField] private GameObject mixButton;
        [SerializeField] private GameObject cocktailCreationArea;
        [SerializeField] private GameObject cocktailServingArea;
        [SerializeField] private GameObject fullnessBar;
        [SerializeField] private GameObject trashButton;
        [SerializeField] private GameObject waterButton;
        [SerializeField] private GameObject serveButton;
        [SerializeField] private Recipes recipes;
        

        
        private readonly Dictionary<IngredientType, int> _ingredientsInShaker = new Dictionary<IngredientType, int>();
        // _validRecipes is a list of couples ( dict<ingrType,int>, cocktailPrefab )
        private readonly List<RecipeItem> _validRecipes = new List<RecipeItem>();
        private GameObject _shaker;
        private RectTransform _cocktailServingAreaRectTransform;
        private Animator _cocktailCreationAreaAnimator;

        private GameObject _cocktail;
        private GameObject _garnish;
        
        private GameObject _ripplePrefab;
        private GameObject _everestPrefab;
        private GameObject _springBeePrefab;
        private GameObject _partiPrefab;
        private GameObject _magazinePrefab;
        private GameObject _wrongPrefab;
        
        private GameObject _rippleGarnishPrefab;
        private GameObject _everestGarnishPrefab;
        private GameObject _springBeeGarnishPrefab;
        private GameObject _partiGarnishPrefab;
        private GameObject _magazineGarnishPrefab;

        private void Start()
        {
            _shaker = GameObject.FindGameObjectWithTag("Shaker");
            _cocktailServingAreaRectTransform = cocktailServingArea.GetComponent<RectTransform>();
            _cocktailCreationAreaAnimator = cocktailCreationArea.GetComponent<Animator>();
            
            // Load prefabs from Resources folder
            _ripplePrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/Ripple");
            _everestPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/Everest");
            _springBeePrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/SpringBee");
            _partiPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/Parti");
            _magazinePrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/Magazine");
            _wrongPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/Wrong");
            
            _rippleGarnishPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/RippleGarnish");
            _everestGarnishPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/EverestGarnish");
            _springBeeGarnishPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/SpringBeeGarnish");
            _partiGarnishPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/PartiGarnish");
            _magazineGarnishPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/MagazineGarnish");

            // Subscribe to events
            EventSystemManager.OnMakeCocktail += ShowArea;
            EventSystemManager.OnIngredientAdded += UpdateFullnessBar;
            EventSystemManager.OnShakerFull += ActivateMixButton;
            EventSystemManager.OnGarnishAdded += ServeCocktail;
            
            // Initialize recipes 
            InitializeRecipes();
        }

        public void ShowArea()
        {
            _cocktailCreationAreaAnimator.SetBool("slideIn", true);
        }

        public void HideArea()
        {
            _cocktailCreationAreaAnimator.SetBool("slideIn", false);
            cocktailServingArea.SetActive(false);
        }
        
        public void ToggleArea()
        {
            if (_cocktailCreationAreaAnimator.GetBool("slideIn")) HideArea();
            else ShowArea();
        }


        private void UpdateFullnessBar(int index, IngredientType ingredient)
        {
            fullnessBar.GetComponent<FullnessBar>().ColorBarSlot(index, ingredient);
        }

        public void MakeCocktail()
        {
            CreateIngredientsDictionary(_shaker.GetComponent<Shaker>().GetIngredients());
            _shaker.GetComponent<Shaker>().EmptyShaker();
            fullnessBar.GetComponent<FullnessBar>().ResetBar();
            DeactivateMixButton();
            
            //debug
            PrintIngredientsDictionary(_ingredientsInShaker);
            
            GameObject cocktailPrefab = _wrongPrefab;

            foreach (RecipeItem ri in _validRecipes)
            {
                PrintIngredientsDictionary(ri.Ingredients);
                if (CheckForValidRecipe(_ingredientsInShaker, ri.Ingredients))
                {
                    cocktailPrefab = ri.CocktailPrefab;
                    break;
                }
            }
            
            _cocktail = Instantiate(cocktailPrefab, spawnPoint.position, spawnPoint.rotation, _cocktailServingAreaRectTransform);
            
        }

        private bool CheckForValidRecipe(Dictionary<IngredientType,int> A, Dictionary<IngredientType,int> B)
        {
            // Check if the dictionaries have the same number of elements
            if (A.Count != B.Count)
                return false;

            // Confront every key-value couple in the dictionaries
            foreach (var kvp in A)
            {
                if (!B.TryGetValue(kvp.Key, out int value) || value != kvp.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public void SpawnGarnish()
        {
            // Deactivate the cocktail serving area buttons
            ToggleCsaButtons(false);
            
            // spawn the correct garnish based on the current cocktail
            if (_cocktail != null)
            {
                // todo: is there a better way to do it?
                GameObject garnishPrefab;
                
                switch (_cocktail.GetComponent<CocktailScript>().cocktail.type)
                {
                    case CocktailType.Everest:
                        garnishPrefab = _everestGarnishPrefab;
                        break;
                    
                    case CocktailType.Magazine:
                        garnishPrefab = _magazineGarnishPrefab;
                        break;
                    
                    case CocktailType.Parti:
                        garnishPrefab = _partiGarnishPrefab;
                        break;
                    
                    case CocktailType.Ripple:
                        garnishPrefab = _rippleGarnishPrefab;
                        break;
                    
                    case CocktailType.SpringBee:
                        garnishPrefab = _springBeeGarnishPrefab;
                        break;
                    
                    default:
                        garnishPrefab = _rippleGarnishPrefab;
                        break;
                }
                
                _garnish = Instantiate(garnishPrefab, garnishSpawnPoint.position,
                    garnishSpawnPoint.rotation, _cocktailServingAreaRectTransform);
            }
        }

        private void ServeCocktail()
        {
            CocktailScript cocktailItem = _cocktail.GetComponent<CocktailScript>();
            EventSystemManager.OnCocktailMade(_cocktail.GetComponent<CocktailScript>().cocktail);
            
            HideArea();
        }
        
        
        public void WaterDownCocktail()
        {
            _cocktail.GetComponent<CocktailScript>().WaterDownCocktail();
        }


        public void TrashCocktail()
        {
            cocktailServingArea.SetActive(false);
            _shaker.GetComponent<Shaker>().EmptyShaker();
            GameObject.Destroy(_cocktail);
            GameObject.Destroy(_garnish);
        }

        private void InitializeRecipes()
        {
            // _validRecipes is a list of RecipeItem, i.e. couples ( Dictionary<IngredientType,int>, GameObject )
            // RecipeItem is a struct containing a Dictionary<IngredientType,int> and a GameObject for the cocktail prefab
            // recipes is a list objects Recipe
            // Recipe is an object containing a List<IngredientType> and a GameObject for the cocktail prefab
            
            foreach (Recipe r in recipes)
            {
                RecipeItem item = new RecipeItem(CreateRecipeDictionary(r.ingredients),r.cocktailPrefab);
                _validRecipes.Add(item);
            }
            
            
            // DEBUG
            //PrintRecipes();
        }

        private void PrintRecipes()
        {
            foreach (RecipeItem r in _validRecipes)
            {
                Debug.Log($"{r.CocktailPrefab}");
                foreach (var entry in r.Ingredients)
                {
                    Debug.Log($"{entry.Key}: {entry.Value}");
                }
            }
        }
        
        private Dictionary<IngredientType,int> CreateRecipeDictionary(List<IngredientType> list)
        {
            Dictionary<IngredientType, int> t = new Dictionary<IngredientType, int>();
            
            foreach (var ingredient in list)
            {
                if (t.ContainsKey(ingredient))
                {
                    t[ingredient]++;
                }
                else
                {
                    t[ingredient] = 1;
                }
            }

            return t;
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
            ingredients.Clear();
        }
        
        private void PrintIngredientsDictionary(Dictionary<IngredientType, int> dictionary)
        {
            Debug.Log("Ingredients in the dictionary:");

            foreach (var entry in dictionary)
            {
                Debug.Log($"{entry.Key}: {entry.Value}");
            }
        }
        
        private void ToggleCsaButtons(bool val)
        {
            trashButton.SetActive(val);
            waterButton.SetActive(val);
            serveButton.SetActive(val);
        }

        public void ActivateCocktailServingArea()
        {
            ToggleCsaButtons(true);
            cocktailServingArea.SetActive(true);
        }

        public void DeactivateCocktailServingArea()
        {
            cocktailServingArea.SetActive(false);
        }

        private void ActivateMixButton()
        {
            mixButton.SetActive(true);
        }
        
        public void DeactivateMixButton()
        {
            mixButton.SetActive(false);
        }
        
    }
    
    public struct RecipeItem
    {
        public Dictionary<IngredientType, int> Ingredients;
        public GameObject CocktailPrefab;

        public RecipeItem(Dictionary<IngredientType, int> ingredients, GameObject cocktailPrefab)
        {
            Ingredients = ingredients;
            CocktailPrefab = cocktailPrefab;
        }
    }
}
