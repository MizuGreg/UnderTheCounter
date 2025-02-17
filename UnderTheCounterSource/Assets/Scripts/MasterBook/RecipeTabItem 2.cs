using Bar;
using CocktailCreation;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MasterBook
{
    public class RecipeTabItem : MonoBehaviour
    {
        public CocktailType cocktailType;
        [FormerlySerializedAs("recipePageManager")] [FormerlySerializedAs("recipeBookManager")] [SerializeField] private RecipeTabManager recipeTabManager;
        private bool selected;
        
        private Sprite emptyTriangle;
        private Sprite blackTriangle;

        private void Start()
        {
            emptyTriangle = Resources.Load<Sprite>("Sprites/UI/MasterBook/RecipeTab/SPRITE SELEZ OFF");
            blackTriangle = Resources.Load<Sprite>("Sprites/UI/MasterBook/RecipeTab/SPRITE SELEZ ON");
            
            // this ensures that the recipe book always opens up with a cocktail in the beginning:
            if (cocktailType == CocktailType.Ripple) ShowThisCocktail();
            else Deselect();
        }
        
        public void ShowThisCocktail()
        {
            selected = true;
            HighlightName();
            recipeTabManager.SetCurrentCocktail(cocktailType);
        }

        public void Deselect()
        {
            selected = false;
            DehighlightName();
        }

        public void PointerEnter()
        {
            if (!selected) HighlightName();
        }

        public void PointerExit()
        {
            if (!selected) DehighlightName();
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