using UnityEngine;

namespace Bar
{
    public class BarManager : MonoBehaviour
    {
        private CustomerManager customerManager;
        private CCAManager CCAManager;
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
            CCAManager = GetComponent<CCAManager>();
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
        
        }

        public void endDay()
        {
            // todo: fade out day, then switch to end of day summary
            print("Day has ended!");
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
