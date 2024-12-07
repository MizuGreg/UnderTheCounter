using Bar;
using CocktailCreation;
using UnityEngine.Events;

namespace Technical
{
    public static class EventSystemManager
    {
        public static UnityAction OnDayStart; // event is called either from shop window view or from bar manager's start()
        public static UnityAction OnDayEnd;
        public static UnityAction OnTimeUp;
    
        public static UnityAction OnCustomerEnter;
        public static UnityAction OnCustomerLeave;
        public static UnityAction OnDrunkCustomerLeave;
        public static UnityAction OnCustomersDepleted;
    
        public static UnityAction OnPreparationStart;
        public static UnityAction OnIngredientPouring;
        public static UnityAction<IngredientType> OnIngredientPoured;
        public static UnityAction<int,IngredientType> OnIngredientAdded;
        public static UnityAction OnShakerFull;
        public static UnityAction OnGarnishAdded;
        public static UnityAction<CocktailType> OnMakeCocktail;
        public static UnityAction<Cocktail> OnCocktailMade;
        public static UnityAction<string> OnOverwritePostIt;

        public static UnityAction OnBlitzCalled;
        public static UnityAction OnBlitzTimerEnded;

        public static UnityAction OnLoadMainMenu;
        public static UnityAction OnLoadBarView;
        public static UnityAction OnLoadShopWindow;
        public static UnityAction OnLoadEndOfDay;
        public static UnityAction OnLoadWinScreen;
        public static UnityAction OnLoadLoseScreen;

        public static UnityAction OnRecipeBookOpened;
        public static UnityAction OnRecipeBookClosed;

        public static UnityAction NextTutorialStep;
        public static UnityAction HideCCA;
        public static UnityAction<IngredientType> MakeIngredientInteractable;
        public static UnityAction MakeAllIngredientsInteractable;
        public static UnityAction OnTutorial1End;
    }
}