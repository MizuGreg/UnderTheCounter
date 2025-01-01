using System.Text.RegularExpressions;
using UnityEngine;
using CocktailCreation;
using Technical;
using TMPro;
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

        public void OpenRecipeBook()
        {
            if (recipeBook.GetComponent<FadeCanvas>().IsFading()) return;
            EventSystemManager.OnRecipeBookOpened();
            recipeBook.GetComponent<FadeCanvas>().FadeIn();
        }

        public void CloseRecipeBook()
        {
            print("Close recipe book");
            if (recipeBook.GetComponent<FadeCanvas>().IsFading()) return;
            EventSystemManager.OnRecipeBookClosed();
            recipeBook.GetComponent<FadeCanvas>().FadeOut();
        }

        public bool IsRecipeBookOpen()
        {
            return recipeBook.gameObject.activeSelf;
        }

        public void SetCurrentCocktail(CocktailType cocktailType)
        {
            currentCocktail = cocktailType;
            ShowCurrentCocktail();
        }

       private string GenerateIngredientsList(CocktailType cocktailType)
       {
            Recipe[] recipes = Resources.FindObjectsOfTypeAll<Recipe>();
            string ingredientsText = "";

            foreach (Recipe recipe in recipes)
            {
                if (recipe.cocktailPrefab.GetComponent<CocktailScript>().cocktail.type == cocktailType)
                {
                    var dictionary = new System.Collections.Generic.Dictionary<IngredientType, int>(5);
                    foreach (IngredientType ingredient in recipe.ingredients)
                    {
                        if (dictionary.ContainsKey(ingredient)) dictionary[ingredient]++;
                        else dictionary.Add(ingredient, 1);
                    }

                    foreach (var pair in dictionary)
                    {
                        ingredientsText += $"{pair.Value}   oz {pair.Key}\n";
                    }

                    return ingredientsText;
                }
            }

            return "No ingredients found.";
        }

        private string RetrieveDescription(CocktailType cocktailType)
        {
            // CocktailScript[] cocktailScripts = Resources.FindObjectsOfTypeAll<CocktailScript>();
            //
            // foreach (CocktailScript script in cocktailScripts)
            // {
            //     if (script.cocktail.type == cocktailType)
            //     {
            //         return script.cocktail.description;
            //     }
            // }
            // return "No description found.";

            Cocktail dummyCocktail = new Cocktail(cocktailType, false);
            return dummyCocktail.description;
        }

        public string FormatCocktailName(string cocktailName)
        {
            return Regex.Replace(cocktailName, @"([a-z])([A-Z])", "$1 $2");
        }

        public void ShowCurrentCocktail()
        {
            foreach (RecipeBookItem recipe in recipes)
            {
                if (recipe.GetComponent<RecipeBookItem>().cocktailType != currentCocktail) recipe.Deselect();
            }
            cocktailName.text = FormatCocktailName(currentCocktail.ToString());
            cocktailSprite.sprite = Resources.Load<Sprite>($"Sprites/Cocktails/{currentCocktail}/{currentCocktail}_tot");
            ingredientsList.text = GenerateIngredientsList(currentCocktail);
            cocktailDescription.text = RetrieveDescription(currentCocktail);
        }
    }
}