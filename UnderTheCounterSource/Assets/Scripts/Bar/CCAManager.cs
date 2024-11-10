using UnityEngine;

namespace Bar
{
    public class CCAManager : MonoBehaviour
    {
        public CanvasGroup CCACanvas;
        public CanvasGroup postItCanvas;

        public void Start()
        {
            EventSystemManager.OnMakeCocktail += setupPreparation;
        }

        public void showCCA()
        {
            // todo
        }

        public void hideCCA()
        {
            // todo
        }

        public void setupPreparation(Cocktail cocktail)
        {
            setupPostIt(cocktail);
            // todo: prepare lil squares, etc.
            showCCA();
        }
    
        // todo: many more functions for ingredients, watering down, mixing etc.

        public void setupPostIt(Cocktail cocktail)
        {
            // todo: write cocktail name on post-it
            showPostIt();
        }
    
        public void showPostIt()
        {
            // todo
        }

        public void hidePostIt()
        {
            // todo
        }

        public void cocktailMade(Cocktail cocktail)
        {
            // todo: throw onCocktailMade. this function is called when the cocktail is placed on the counter
            EventSystemManager.OnCocktailMade(cocktail);
        }
    }
}
