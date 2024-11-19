using UnityEngine;

namespace CocktailCreation
{
    public class CocktailScript : MonoBehaviour
    {
        public Cocktail cocktail;
        [SerializeField] private Sprite sprite;

        public void WaterDownCocktail()
        {
            cocktail.isWatered = true;
            print("Cocktail watered");
        }

    }
}
