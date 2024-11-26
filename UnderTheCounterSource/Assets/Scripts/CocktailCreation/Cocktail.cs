using UnityEngine;

namespace CocktailCreation
{
    [CreateAssetMenu(fileName = "new Cocktail", menuName = "Cocktail")]
    public class Cocktail : ScriptableObject
    {
        public CocktailType type;
        public bool isWatered;
        public string description;
        

        public Cocktail(CocktailType type, bool isWatered)
        {
            this.type = type;
            this.isWatered = isWatered;

            string description;
            switch (type) {
                case CocktailType.Ripple:
                    description = "(placeholder description!!!) A classic cocktail made with tequila, triple sec, and lime juice.";
                    break;
                case CocktailType.Everest:
                    description = "(placeholder description!!!) A traditional Cuban highball made with white rum, sugar, lime juice, soda water, and mint.";
                    break;
                case CocktailType.SpringBee:
                    description = "(placeholder description!!!) A sweet cocktail made with rum, coconut cream or coconut milk, and pineapple juice.";
                    break;
                case CocktailType.Parti:
                    description = "(placeholder description!!!) A family of cocktails whose main ingredients are rum, citrus juice, and sugar.";
                    break;
                case CocktailType.Magazine:
                    description = "(placeholder description!!!) A cocktail made with rum, lime juice, orgeat syrup, and orange liqueur.";
                    break;
                default:
                    description = "(placeholder description!!!) A cocktail made with rum, lime juice, orgeat syrup, and orange liqueur.";
                    break;
            }
            this.description = description;
        }
    }
}