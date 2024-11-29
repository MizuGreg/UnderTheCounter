using CocktailCreation;
using UnityEngine;

namespace Bar
{
    public class RecipeBookItem : MonoBehaviour
    {
        [SerializeField] private CocktailType cocktailType;
        [SerializeField] private RecipeBookManager recipeBookManager;

        public void SetCurrentCocktailToThis()
        {
            recipeBookManager.SetCurrentCocktail(cocktailType);
        }
    }
}