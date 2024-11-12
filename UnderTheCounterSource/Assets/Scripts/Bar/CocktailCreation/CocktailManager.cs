using System.Collections.Generic;
using UnityEngine;

namespace Bar.CocktailCreation
{
    public class CocktailManager : MonoBehaviour
    {
        [SerializeField] private RectTransform spawnPoint;
        [SerializeField] private GameObject mixButton;
        [SerializeField] private GameObject cocktailCreationArea;
        [SerializeField] private GameObject cocktailServingArea;
        [SerializeField] private bool slideInArea = false;
        [SerializeField] private GameObject fullnessBar;

        
        private readonly Dictionary<IngredientType, int> _ingredientsInShaker = new Dictionary<IngredientType, int>();
        private GameObject _shaker;
        private RectTransform _cocktailServingAreaRectTransform;
        private Animator _cocktailCreationAreaAnimator;

        private GameObject _cocktail;
        
        private GameObject _ripplePrefab;
        private GameObject _everestPrefab;
        private GameObject _springBeePrefab;
        private GameObject _partiPrefab;
        private GameObject _magazinePrefab;

        private void Start()
        {
            _shaker = GameObject.FindGameObjectWithTag("Shaker");
            _cocktailServingAreaRectTransform = cocktailServingArea.GetComponent<RectTransform>();
            _cocktailCreationAreaAnimator = cocktailCreationArea.GetComponent<Animator>();
            
            // Carica il prefab dalla cartella Resources
            _ripplePrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/Ripple");
            _everestPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/Everest");
            _springBeePrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/SpringBee");
            _partiPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/Parti");
            _magazinePrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/Magazine");

            EventSystemManager.OnMakeCocktail += showArea;

        }


        private void Update()
        {
            if (slideInArea) _cocktailCreationAreaAnimator.SetBool("slideIn", true);
            else _cocktailCreationAreaAnimator.SetBool("slideIn", false);
        }

        public void showArea()
        {
            // todo: show cocktail creation area
            slideInArea = true;
        }

        public void hideArea()
        {
            // todo: hide cocktail creation area
            // this also hides the overlay that displayed the cocktail in big (we need to reset it somehow maybe)
        }

        public void UpdateFullnessBar(int index, IngredientType ingredient)
        {
            fullnessBar.GetComponent<FullnessBar>().ColorBarSlot(index,ingredient);
        }

        public void MakeCocktail()
        {
            CreateIngredientsDictionary(_shaker.GetComponent<Shaker>().GetIngredients());
            _shaker.GetComponent<Shaker>().EmptyShaker();
            fullnessBar.GetComponent<FullnessBar>().ResetBar();
            DeactivateMixButton();
            
            //debug
            //PrintIngredientsDictionary();

            // todo: make Cocktail object and call event with that as parameter
            // todo: use a json file instead of hardcoding recipes here
            
            if (_ingredientsInShaker[IngredientType.Verlan] == 2 && _ingredientsInShaker[IngredientType.Shaddock] == 2
                                                                 && _ingredientsInShaker[IngredientType.Gryte] == 1)
            {
                _cocktail = Instantiate(_ripplePrefab, spawnPoint.position, spawnPoint.rotation, _cocktailServingAreaRectTransform);
            }
            else if (_ingredientsInShaker[IngredientType.Caledon] == 3 && _ingredientsInShaker[IngredientType.Gryte] == 2)
            {
                _cocktail = Instantiate(_everestPrefab, spawnPoint.position, spawnPoint.rotation, _cocktailServingAreaRectTransform);
            }
            else if (_ingredientsInShaker[IngredientType.Verlan] == 3 && _ingredientsInShaker[IngredientType.Shaddock] == 2)
            {
                _cocktail = Instantiate(_springBeePrefab, spawnPoint.position, spawnPoint.rotation, _cocktailServingAreaRectTransform);
            }
            else if (_ingredientsInShaker[IngredientType.Verlan] == 2 && _ingredientsInShaker[IngredientType.Ferrucci] == 3)
            {
                _cocktail = Instantiate(_partiPrefab, spawnPoint.position, spawnPoint.rotation, _cocktailServingAreaRectTransform);
            }
            else if (_ingredientsInShaker[IngredientType.Verlan] == 3 && _ingredientsInShaker[IngredientType.Ferrucci] == 1
                                                                      && _ingredientsInShaker[IngredientType.Shaddock] == 1)
            {
                _cocktail = Instantiate(_magazinePrefab, spawnPoint.position, spawnPoint.rotation, _cocktailServingAreaRectTransform);
            }
            else
            {
                _shaker.SetActive(true);
            }
            
        }

        public void cocktailMade()
        {
            // prende il cocktail attualmente fatto
            // lancia evento onCocktailMade<il cocktail in questione>
            // chiama inoltre questa funzione:
            hideArea();
        }
        
        
        public void WaterDownCocktail()
        {
            _cocktail.GetComponent<Cocktail>().WaterDownCocktail();
        }


        public void TrashCocktail()
        {
            cocktailServingArea.SetActive(false);
            _shaker.GetComponent<Shaker>().EmptyShaker();
            GameObject.Destroy(_cocktail);
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

        public void ActivateCocktailServingArea()
        {
            cocktailServingArea.SetActive(true);
        }

        public void DeactivateCocktailServingArea()
        {
            cocktailServingArea.SetActive(false);
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
