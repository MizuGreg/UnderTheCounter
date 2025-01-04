using System;
using System.Collections.Generic;
using Bar;
using Technical;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private FadeCanvas waterDownIcon;
        [SerializeField] private GameObject serveButton;
        [SerializeField] private Recipes recipes;

        [SerializeField] private PostIt postIt;
        
        
        private static readonly int SlideIn = Animator.StringToHash("slideIn");
        
        private readonly Dictionary<IngredientType, int> _ingredientsInShaker = new Dictionary<IngredientType, int>();
        // _validRecipes is a list of RecipeItem which are couples ( Dictionary<IngredientType,int>, cocktailPrefab )
        private readonly List<RecipeItem> _validRecipes = new List<RecipeItem>();
        private GameObject _shaker;
        private RectTransform _cocktailServingAreaRectTransform;
        private Animator _cocktailCreationAreaAnimator;

        private GameObject _cocktail;
        private GameObject _garnish;
        
        private GameObject _wrongPrefab;
        

        private void Start()
        {
            _shaker = GameObject.FindGameObjectWithTag("Shaker");
            _cocktailServingAreaRectTransform = cocktailServingArea.GetComponent<RectTransform>();
            _cocktailCreationAreaAnimator = cocktailCreationArea.GetComponent<Animator>();
            
            // Load prefabs from Resources folder
            _wrongPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/Wrong");
            

            // Subscribe to events
            EventSystemManager.OnMakeCocktail += ShowArea;
            EventSystemManager.OnIngredientAdded += UpdateFullnessBar;
            EventSystemManager.OnShakerFull += ActivateMixButton;
            EventSystemManager.OnGarnishAdded += ServeCocktail;
            
            EventSystemManager.HideCCA += HideArea;
            
            // Initialize recipes 
            InitializeRecipes();
            mixButton.GetComponent<Button>().interactable = false;
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            EventSystemManager.OnMakeCocktail -= ShowArea;
            EventSystemManager.OnIngredientAdded -= UpdateFullnessBar;
            EventSystemManager.OnShakerFull -= ActivateMixButton;
            EventSystemManager.OnGarnishAdded -= ServeCocktail;

            EventSystemManager.HideCCA -= HideArea;
        }

        private void ShowArea(CocktailType cocktailType)
        {
            _cocktailCreationAreaAnimator.SetBool(SlideIn, true);
            if (cocktailType != CocktailType.Wrong) postIt.ShowPostIt();
        }

        private void HideArea()
        {
            _cocktailCreationAreaAnimator.SetBool(SlideIn, false);
            DeactivateCocktailServingArea();
            postIt.HidePostIt();
        }
        
        public void ToggleArea()
        {
            if (_cocktailCreationAreaAnimator.GetBool(SlideIn)) HideArea();
            else ShowArea(CocktailType.Wrong);
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
            
            // Debug
            //PrintIngredientsDictionary(_ingredientsInShaker);
            
            GameObject cocktailPrefab = _wrongPrefab;

            foreach (RecipeItem ri in _validRecipes)
            {
                // Debug
                //PrintIngredientsDictionary(ri.Ingredients);
                if (CheckForValidRecipe(_ingredientsInShaker, ri.Ingredients))
                {
                    cocktailPrefab = ri.CocktailPrefab;
                    break;
                }
            }
            
            _cocktail = Instantiate(cocktailPrefab, spawnPoint.position, spawnPoint.rotation, _cocktailServingAreaRectTransform);
            
        }

        private bool CheckForValidRecipe(Dictionary<IngredientType,int> a, Dictionary<IngredientType,int> b)
        {
            // Check if the dictionaries have the same number of elements
            if (a.Count != b.Count)
                return false;

            // Confront every key-value couple in the dictionaries
            foreach (var kvp in a)
            {
                if (!b.TryGetValue(kvp.Key, out int value) || value != kvp.Value)
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
            _garnish = Instantiate(_cocktail.GetComponent<CocktailScript>().GetGarnish(), garnishSpawnPoint.position,
                    garnishSpawnPoint.rotation, _cocktailServingAreaRectTransform);
            _garnish.GetComponent<FadeCanvas>().FadeIn();
            
        }

        private void ServeCocktail()
        {
            CocktailScript cocktailItem = _cocktail.GetComponent<CocktailScript>();
            EventSystemManager.OnCocktailMade(_cocktail.GetComponent<CocktailScript>().cocktail);
            
            TrashCocktail();
            HideArea();
        }
        
        
        public void WaterDownCocktail()
        {
            _cocktail.GetComponent<CocktailScript>().WaterDownCocktail();
            waterDownIcon.FadeIn();
            waterButton.GetComponent<Button>().interactable = false;
        }


        public void TrashCocktail()
        {
            DeactivateCocktailServingArea();
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
            
            // Debug
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
            _ingredientsInShaker.Clear();
            
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
            waterButton.GetComponent<Button>().interactable = val; // otherwise the button is still not interactable if it has been watered in the previous customer
            serveButton.SetActive(val);
        }

        public void ActivateCocktailServingArea()
        {
            ToggleCsaButtons(true);
            cocktailServingArea.GetComponent<FadeCanvas>().FadeIn();
            waterDownIcon.gameObject.SetActive(false);
        }

        public void DeactivateCocktailServingArea()
        {
            if (cocktailServingArea.gameObject.activeInHierarchy)
            {
                if (waterDownIcon.gameObject.activeInHierarchy)
                {
                    waterDownIcon.FadeOut();
                }
                cocktailServingArea.GetComponent<FadeCanvas>().FadeOut();
                
            }
            
        }

        private void ActivateMixButton()
        {
            mixButton.GetComponent<Button>().interactable = true;
        }
        
        public void DeactivateMixButton()
        {
            mixButton.GetComponent<Button>().interactable = false;
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
