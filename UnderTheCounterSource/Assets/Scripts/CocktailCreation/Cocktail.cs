using UnityEngine;

namespace CocktailCreation
{
    public class Cocktail : MonoBehaviour
    {
        [SerializeField] private CocktailType cocktailType;

        private bool _isWatered = false;

        public Cocktail(CocktailType cocktailType, bool isWatered)
        {
            this.cocktailType = cocktailType;
            this._isWatered = isWatered;
        }

        public void WaterDownCocktail()
        {
            _isWatered = true;
            Debug.Log("Cocktail watered");
        }

    }
}
