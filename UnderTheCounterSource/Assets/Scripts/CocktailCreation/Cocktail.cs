using System;
using UnityEngine;

namespace CocktailCreation
{
    public class Cocktail : MonoBehaviour
    {
        [SerializeField] private CocktailType cocktailType;

        private bool _isWatered = false;

        public void WaterDownCocktail()
        {
            _isWatered = true;
            Debug.Log("Cocktail watered");
        }

    }
}
