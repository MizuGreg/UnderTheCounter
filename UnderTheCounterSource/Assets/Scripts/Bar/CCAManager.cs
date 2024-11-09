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
        showCCA();
        setupPostIt(cocktail);
        // todo: prepare lil squares, etc.
        showCCA();
    }
    
    // todo: many more functions for ingredients, watering down, mixing etc.

    public void setupPostIt(Cocktail cocktail)
    {
        // todo
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
        // todo: throw onCocktailMade
    }
}
