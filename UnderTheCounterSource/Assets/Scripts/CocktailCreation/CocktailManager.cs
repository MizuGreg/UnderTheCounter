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
        

        
        private readonly Dictionary<IngredientType, int> _ingredientsInShaker = new Dictionary<IngredientType, int>();
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
            
            // Carica il prefab dalla cartella Resources
            _ripplePrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/Ripple");
            _everestPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/Everest");
            _springBeePrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/SpringBee");
            _partiPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/Parti");
            _magazinePrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/Magazine");
            
            _rippleGarnishPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/RippleGarnish");
            _everestGarnishPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/EverestGarnish");
            _springBeeGarnishPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/SpringBeeGarnish");
            _partiGarnishPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/PartiGarnish");
            _magazineGarnishPrefab = Resources.Load<GameObject>("Prefabs/CocktailCreation/MagazineGarnish");

            EventSystemManager.OnMakeCocktail += ShowArea;
            EventSystemManager.OnIngredientAdded += UpdateFullnessBar;
            EventSystemManager.OnShakerFull += ActivateMixButton;
            EventSystemManager.OnGarnishAdded += ServeCocktail;

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
            //PrintIngredientsDictionary();

            // todo: use a json file instead of hardcoding recipes here
            
            // todo: fix this (it has to spawn the default wrong cocktail, not ripple)
            GameObject cocktailPrefab = _ripplePrefab;
            
            // Ripple
            if (_ingredientsInShaker[IngredientType.Verlan] == 2 && _ingredientsInShaker[IngredientType.Shaddock] == 2
                                                                 && _ingredientsInShaker[IngredientType.Gryte] == 1)
            {
                cocktailPrefab = _ripplePrefab;
            }
            // Everest
            else if (_ingredientsInShaker[IngredientType.Caledon] == 3 && _ingredientsInShaker[IngredientType.Gryte] == 2)
            {
                cocktailPrefab = _everestPrefab;
            }
            // SpringBee
            else if (_ingredientsInShaker[IngredientType.Verlan] == 3 && _ingredientsInShaker[IngredientType.Shaddock] == 2)
            {
                cocktailPrefab = _springBeePrefab;
            }
            // Parti
            else if (_ingredientsInShaker[IngredientType.Verlan] == 2 && _ingredientsInShaker[IngredientType.Ferrucci] == 3)
            {
                cocktailPrefab = _partiPrefab;
            }
            // Magazine
            else if (_ingredientsInShaker[IngredientType.Verlan] == 3 && _ingredientsInShaker[IngredientType.Ferrucci] == 1
                                                                      && _ingredientsInShaker[IngredientType.Shaddock] == 1)
            {
                cocktailPrefab = _magazinePrefab;
            }
            
            _cocktail = Instantiate(cocktailPrefab, spawnPoint.position, spawnPoint.rotation, _cocktailServingAreaRectTransform);
            
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
            Debug.Log("Ingredients in the dictionary:");

            foreach (var entry in _ingredientsInShaker)
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
}
