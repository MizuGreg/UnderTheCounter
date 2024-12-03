using CocktailCreation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bar
{
    public class RecipeBookItem : MonoBehaviour
    {
        public CocktailType cocktailType;
        [SerializeField] private RecipeBookManager recipeBookManager;
        
        private Sprite emptyTriangle;
        private Sprite blackTriangle;

        private void Start()
        {
            emptyTriangle = Resources.Load<Sprite>("Sprites/UI/Recipe book/SPRITE SELEZ OFF");
            blackTriangle = Resources.Load<Sprite>("Sprites/UI/Recipe book/SPRITE SELEZ ON");
            
            // this ensures that the recipe book always opens up with a cocktail in the beginning:
            if (cocktailType == CocktailType.Everest) ShowThisCocktail();
        }
        
        public void ShowThisCocktail()
        {
            recipeBookManager.SetCurrentCocktail(cocktailType);
        }

        public void HighlightName()
        {
            this.transform.Find("Triangle").GetComponent<Image>().sprite = blackTriangle;
        }
        
        public void DehighlightName()
        {
            this.transform.Find("Triangle").GetComponent<Image>().sprite = emptyTriangle;
        }
    }
}