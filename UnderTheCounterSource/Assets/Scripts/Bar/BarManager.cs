using Bar.CocktailCreation;
using UnityEngine;

namespace Bar
{
    public class BarManager : MonoBehaviour
    {
        private CustomerManager customerManager;
        private CocktailManager cocktailManager;
        private RecipeBookManager recipeBookManager;
        private TimerManager timerManager;
        private DialogueManager dialogueManager;

        public CanvasGroup barContainer;
        public CanvasGroup barCanvas;
        // todo: pocket watch canvas
    
        // Start is called before the first frame update
        void Start()
        {
            customerManager = GetComponent<CustomerManager>();
            cocktailManager = GetComponent<CocktailManager>();
            recipeBookManager = GetComponent<RecipeBookManager>();
            timerManager = GetComponent<TimerManager>();
            dialogueManager = GetComponent<DialogueManager>();
            customerManager.attachDialogueManager(dialogueManager);
        
            EventSystemManager.OnDayStart += startDay;
            EventSystemManager.OnDrunkCustomerLeave += checkDrunk;
            EventSystemManager.OnCustomersDepleted += endDay;
        
            barContainer.GetComponent<CanvasFadeAnimation>().FadeIn();
        }

        public void startDay()
        {
            if (DaySO.currentDay == 1) customerManager.playTutorial();
            else customerManager.greetCustomer();
            timerManager.startTimer();
            print($"Timer has started for day {DaySO.currentDay}");
        
        }

        public void endDay()
        {
            // todo: fade out day, then switch to end of day summary
            print($"Day has ended! Today's earnings: {DaySO.todayEarnings}");
            DaySO.savings += DaySO.todayEarnings;
            DaySO.todayEarnings = 0;
            EventSystemManager.OnDayEnd();
            DaySO.currentDay++;
        }

        private void checkDrunk()
        {
            if (DaySO.drunkCustomers++ >= DaySO.maxDrunkCustomers) callBlitz();
        }

        private void callBlitz()
        {
            // todo blitz. should probably be a coroutine
            // for now:
            lossByBlitz();
        }

        private void lossByBlitz()
        {
            // todo: display actual loss screen
            print("Loss By Blitz");
        }

        public void quitGame()
        {
            Technical.QuitGame.Quit();
        }
    }
}
