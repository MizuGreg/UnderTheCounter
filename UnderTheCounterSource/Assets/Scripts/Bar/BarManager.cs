using Bar.CocktailCreation;
using UnityEngine;

namespace Bar
{
    public class BarManager : MonoBehaviour
    {
        private CustomerManager _customerManager;
        private CocktailManager _cocktailManager;
        private RecipeBookManager _recipeBookManager;
        private TimerManager _timerManager;
        private DialogueManager _dialogueManager;

        public CanvasGroup barContainer;
        public CanvasGroup barCanvas;
        // todo: pocket watch canvas
    
        // Start is called before the first frame update
        void Start()
        {
            _customerManager = GetComponent<CustomerManager>();
            _cocktailManager = GetComponent<CocktailManager>();
            _recipeBookManager = GetComponent<RecipeBookManager>();
            _timerManager = GetComponent<TimerManager>();
            _dialogueManager = GetComponent<DialogueManager>();
            _customerManager.AttachDialogueManager(_dialogueManager);
        
            EventSystemManager.OnDayStart += StartDay;
            EventSystemManager.OnDrunkCustomerLeave += CheckDrunk;
            EventSystemManager.OnCustomersDepleted += EndDay;
        
            barContainer.GetComponent<CanvasFadeAnimation>().FadeIn();
            EventSystemManager.OnLoadBarView();
        }

        public void StartDay()
        {
            _customerManager.StartDay();
            _timerManager.startTimer();
            print($"Timer has started for day {Day.currentDay}");
        
        }

        private void EndDay()
        {
            // todo: fade out day, then switch to end of day summary
            print($"Day has ended! Today's earnings: {Day.todayEarnings}");
            Day.savings += Day.todayEarnings;
            Day.todayEarnings = 0;
            EventSystemManager.OnDayEnd();
            Day.currentDay++;
        }

        private void CheckDrunk()
        {
            if (Day.drunkCustomers++ >= Day.maxDrunkCustomers) CallBlitz();
        }

        private void CallBlitz()
        {
            // todo blitz. should probably be a coroutine
            // for now:
            LossByBlitz();
        }

        private void LossByBlitz()
        {
            // todo: display actual loss screen
            print("Loss By Blitz");
        }

        public void backToMainMenu()
        {
            // todo
        }

        public void QuitGame()
        {
            Technical.QuitGame.Quit();
        }
    }
}
