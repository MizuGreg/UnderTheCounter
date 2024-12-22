using UnityEngine;
using UnityEngine.EventSystems;

namespace CocktailCreation
{
    public class CocktailScript : MonoBehaviour
    {
        public Cocktail cocktail;
        [SerializeField] private Sprite sprite;
        [SerializeField] private GameObject garnish;

        public void WaterDownCocktail()
        {
            cocktail.isWatered = true;
            Debug.Log("cocktail watered");
        }

        public GameObject GetGarnish()
        {
            return this.garnish;
        }

    }
}
