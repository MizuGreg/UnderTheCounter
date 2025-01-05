using Bar;
using CocktailCreation;
using UnityEngine.Events;

namespace Technical
{
    public static class EventSystemManager
    {
        // Poster events
        public static UnityAction OnPosterBought;
        
        // Day-related events
        public static UnityAction OnDayStart;
        public static UnityAction OnDayEnd;
        public static UnityAction OnTimeUp;
    
        // Customer-related events
        public static UnityAction OnCustomerEnter;
        public static UnityAction OnCustomerLeave;
        public static UnityAction OnDrunkCustomer;
        public static UnityAction OnCustomerLeft;
        public static UnityAction OnCustomersDepleted;
    
        // Cocktail-related events
        public static UnityAction OnPreparationStart;
        public static UnityAction OnIngredientPouring;
        public static UnityAction<IngredientType> OnIngredientPoured;
        public static UnityAction<int,IngredientType> OnIngredientAdded;
        public static UnityAction OnShakerFull;
        public static UnityAction OnGarnishAdded;
        public static UnityAction<CocktailType> OnMakeCocktail;
        public static UnityAction<Cocktail> OnCocktailMade;
        public static UnityAction<string> OnWritePostIt;

        // Blitz-related events
        public static UnityAction OnBlitzCalled;
        public static UnityAction OnBlitzCallWarning;
        public static UnityAction OnBlitzTimerEnded;
        public static UnityAction OnBottlePlaced;
        public static UnityAction OnPanelOpened;
        public static UnityAction OnMinigameEnd;

        public static UnityAction MultipleChoiceStart;

        public static UnityAction OnBlitzEnd;

        // Scene loads
        public static UnityAction OnLoadMainMenu;
        public static UnityAction OnLoadBarView;
        public static UnityAction OnLoadShopWindow;
        public static UnityAction OnLoadEndOfDay;
        public static UnityAction OnLoadWinScreen;
        public static UnityAction OnLoadLoseScreen;

        // Master book events
        public static UnityAction OnMasterBookOpened;
        public static UnityAction OnMasterBookClosed;
        public static UnityAction OnTabChanged;
        public static UnityAction OnPageTurned;

        // Tutorial events
        public static UnityAction NextTutorialStep;
        public static UnityAction HideCCA;
        public static UnityAction<IngredientType> MakeIngredientInteractable;
        public static UnityAction MakeAllIngredientsInteractable;
        public static UnityAction OnTutorial1End;

        // Game events
        public static UnityAction<int> OnTrinketObtained;
        public static UnityAction<int> OnPosterObtained;
        
        // Achievements events
        public static UnityAction<string, int> OnAchievementProgress;
    }
}