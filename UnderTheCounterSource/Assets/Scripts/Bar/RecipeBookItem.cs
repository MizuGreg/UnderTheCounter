using CocktailCreation;
using UnityEngine;

namespace Bar
{
    public class RecipeBookItem : MonoBehaviour
    {
        [SerializeField] private CocktailType cocktailType;
        [SerializeField] private RecipeBookManager recipeBookManager;

        public void ShowThisCocktail()
        {
            recipeBookManager.SetCurrentCocktail(cocktailType);
            recipeBookManager.ShowCurrentCocktail();
        }
    }
}