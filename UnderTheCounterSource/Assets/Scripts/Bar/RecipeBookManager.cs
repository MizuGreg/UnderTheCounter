using UnityEngine;
using CocktailCreation;

namespace Bar
{
    public class RecipeBookManager : MonoBehaviour
    {
        public GameObject recipeBook;
        public Cocktail currentCocktail = null;

        void Start()
        {
            recipeBook.SetActive(false);
        }

        public void ShowRecipeBook()
        {
            recipeBook.SetActive(true);
            
        }

        public void ShowCocktail(Cocktail cocktail)
        {
            currentCocktail = cocktail;
            // wip
        }
        
    }
}