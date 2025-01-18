using Bar;
using CocktailCreation;
using UnityEngine.Events;

namespace Technical
{
    public static class EventSystemManager
    {
        // Poster events
        public static UnityAction OnPosterBought;
        public static UnityAction OnPosterHung;
        public static UnityAction OnPosterRippedDown;
        
        // Day-related events
        public static UnityAction OnDayStart;
        public static UnityAction OnDayEnd;
        public static UnityAction OnTimeWarning;
        public static UnityAction OnTimeUp;
    
        // Customer-related events
        public static UnityAction OnCustomerEnter;
        public static UnityAction OnCustomerLeave;
        public static UnityAction OnCustomerLeaveSound;
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
        public static UnityAction OnHowardEnter;
        public static UnityAction OnWrongChoice;

        // Scene loads
        public static UnityAction OnLoadMainMenu;
        public static UnityAction OnLoadBarView;
        public static UnityAction OnLoadShopWindow;
        public static UnityAction OnLoadEndOfDay;
        public static UnityAction OnLoadWinScreen;
        public static UnityAction<string> OnLoadLoseScreen;

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
        public static UnityAction OnAchievementUnlocked;
        public static UnityAction OnTutorialCompleted;
        public static UnityAction OnDealMade;
        public static UnityAction OnHalfTrinketCollected;
        public static UnityAction OnBankrupt;
        public static UnityAction OnBlitzLose;
        public static UnityAction OnWin;
        public static UnityAction OnButterfly1;
        public static UnityAction OnButterfly2;
        public static UnityAction OnBarBurned;
        public static UnityAction OnDealRefused;
        public static UnityAction OnCocktailWatered;
        public static UnityAction OnBackstabbed;
        public static UnityAction OnAllTrinketCollected;
        public static UnityAction OnAllCustomersServed;
        
        // Trinkets events
        public static UnityAction<int> OnTrinketCollected;
        public static UnityAction<int> OnTrinketDisplayed;
        
        // Easter Egg event
        public static UnityAction OnNapoli;

        // Final day event
        public static UnityAction OnErnestReveal;

    }
}