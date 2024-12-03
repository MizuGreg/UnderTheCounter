using System;
using UnityEngine;
using CocktailCreation;
using Technical;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Bar
{
    public class RecipeBookManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup recipeBook;
        [SerializeField] private CocktailType currentCocktail;
        [SerializeField] private RecipeBookItem[] recipes;

        [SerializeField] private CanvasGroup currentCocktailCanvas;
        [SerializeField] private TextMeshProUGUI cocktailName;
        [SerializeField] private Image cocktailSprite;
        [SerializeField] private TextMeshProUGUI ingredientsList;
        [SerializeField] private TextMeshProUGUI cocktailDescription;

        void Start()
        {
            recipeBook.gameObject.SetActive(false);
        }

        public void SetCurrentCocktail(CocktailType cocktailType)
        {
            currentCocktail = cocktailType;
            ShowCurrentCocktail();
        }

        private string GenerateIngredientsList(CocktailType cocktailType)
        {
            Recipe example = Resources.FindObjectsOfTypeAll<Recipe>()[0];
            print(example.ingredients);

            string ingredients;
            // hardcoded for now
            switch (cocktailType)
            {
                case CocktailType.Ripple:
                    ingredients = "- 1 oz Verlan<br>- 2 oz Caledon Ridge<br>- 2 oz shaddock juice";
                    break;
                case CocktailType.Everest:
                    ingredients = "- 1 oz Verlan<br>- 2 oz Caledon Ridge<br>- 2 oz shaddock juice";
                    break;
                case CocktailType.SpringBee:
                    ingredients = "- 1 oz Verlan<br>- 2 oz Caledon Ridge<br>- 2 oz shaddock juice";
                    break;
                case CocktailType.Parti:
                    ingredients = "- 1 oz Verlan<br>- 2 oz Caledon Ridge<br>- 2 oz shaddock juice";
                    break;
                case CocktailType.Magazine:
                    ingredients = "- 1 oz Verlan<br>- 2 oz Caledon Ridge<br>- 2 oz shaddock juice";
                    break;
                case CocktailType.Wrong:
                    ingredients = "- 1 oz Verlan<br>- 2 oz Caledon Ridge<br>- 2 oz shaddock juice";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(cocktailType), cocktailType, null);
            }
            
            return ingredients;
        }

        private string RetrieveDescription(CocktailType cocktailType)
        {
            CocktailScript exampleScript = Resources.FindObjectsOfTypeAll<CocktailScript>()[0];
            print(exampleScript.cocktail.description);
            
            // hardcoded for now
            string description;
            switch (cocktailType) {
                case CocktailType.Ripple:
                    description = "(placeholder description!!!) A classic cocktail made with tequila, triple sec, and lime juice.";
                    break;
                case CocktailType.Everest:
                    description = "(placeholder description!!!) A traditional Cuban highball made with white rum, sugar, lime juice, soda water, and mint.";
                    break;
                case CocktailType.SpringBee:
                    description = "(placeholder description!!!) A sweet cocktail made with rum, coconut cream or coconut milk, and pineapple juice.";
                    break;
                case CocktailType.Parti:
                    description = "(placeholder description!!!) A family of cocktails whose main ingredients are rum, citrus juice, and sugar.";
                    break;
                case CocktailType.Magazine:
                    description = "(placeholder description!!!) A cocktail made with rum, lime juice, orgeat syrup, and orange liqueur.";
                    break;
                default:
                    description = "(placeholder description!!!) A cocktail made with rum, lime juice, orgeat syrup, and orange liqueur.";
                    break;
            }
            return description;
        }

        public void ShowCurrentCocktail()
        {
            foreach (RecipeBookItem recipe in recipes)
            {
                if (recipe.GetComponent<RecipeBookItem>().cocktailType == currentCocktail) recipe.HighlightName();
                else recipe.DehighlightName();
            }
            cocktailName.text = currentCocktail.ToString();
            cocktailSprite.sprite = Resources.Load<Sprite>($"Sprites/CocktailCreation/{currentCocktail}");
            ingredientsList.text = GenerateIngredientsList(currentCocktail);
            cocktailDescription.text = RetrieveDescription(currentCocktail);
        }

        public void TriggerEventRecipeBookOpened()
        {
            EventSystemManager.OnRecipeBookOpened();
        }

        public void TriggerEventRecipeBookClosed()
        {
            EventSystemManager.OnRecipeBookClosed();
        }
    }
}