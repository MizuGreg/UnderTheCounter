using UnityEngine;

namespace CocktailCreation
{
    [CreateAssetMenu(fileName = "new Cocktail", menuName = "Cocktail")]
    public class Cocktail : ScriptableObject
    {
        public CocktailType type;
        public bool isWatered;
        

        public Cocktail(CocktailType type, bool isWatered)
        {
            this.type = type;
            this.isWatered = isWatered;
        }
    }
}