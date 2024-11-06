using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCAManager : MonoBehaviour
{
    public CanvasGroup CCACanvas;
    public CanvasGroup postItCanvas;
    
    public enum Ingredient
    {
        Verlan,
        CaledonRidge,
        Ferrucci,
        Gryte,
        ShaddockJuice,
        Water
    }

    public struct Cocktail
    {
        private CocktailType type;
        private bool wateredDown;
    }

    public enum CocktailType
    {
        Ripple,
        Everest,
        SpringBee,
        Parti,
        Magazine,
        Mistake
    }

    public void Start()
    {
        EventSystemManager.onMakeCocktail += setupPreparation;
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
        showCCA();
        showPostIt(cocktail);
        // todo: prepare lil squares, etc.
    }
    
    // todo: many more functions for ingredients, watering down, mixing etc.

    public void showPostIt(Cocktail cocktail)
    {
        
    }

    public void cocktailMade(Cocktail cocktail)
    {
        // todo: throw onCocktailMade
    }
}
