using System;
using UnityEngine;
using CocktailCreation;
using Technical;
using TMPro;
using UnityEngine.UI;

namespace Bar
{
    public class RecipeBookManager : MonoBehaviour
    {
        public CanvasGroup recipeBook;
        public CocktailType currentCocktail;

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
        }

        public void ShowRecipeBook()
        {
            recipeBook.GetComponent<FadeCanvas>().FadeIn();

        }

        public void HideRecipeBook()
        {
            recipeBook.GetComponent<FadeCanvas>().FadeOut();
        }

        public void ShowCurrentCocktail()
        {
            currentCocktailCanvas.GetComponent<FadeCanvas>().FadeIn();
            // this switch case will become generalized later on
            switch (currentCocktail)
            {
                case CocktailType.Ripple:
                    cocktailName.text = "Ripple";
                    cocktailSprite.sprite = Resources.Load<Sprite>("Sprites/CocktailCreation/Ripple");
                    ingredientsList.text = "1 oz whatever<br>2 oz whatever<br>2 oz whatever";
                    cocktailDescription.text =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip.";
                    break;
                case CocktailType.Everest:
                    cocktailName.text = "Everest";
                    cocktailSprite.sprite = Resources.Load<Sprite>($"Sprites/CocktailCreation/Everest");
                    ingredientsList.text = "1 oz caledon<br>2 oz drugs<br>2 oz water";
                    cocktailDescription.text =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip";
                    break;
                case CocktailType.SpringBee:
                    break;
                case CocktailType.Parti:
                    break;
                case CocktailType.Magazine:
                    break;
                case CocktailType.Wrong:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}