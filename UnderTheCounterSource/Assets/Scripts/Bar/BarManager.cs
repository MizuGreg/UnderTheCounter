using System.Collections;
using Technical;
using Tutorial;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bar
{
    public class BarManager : MonoBehaviour
    {
        private TutorialManager1 _tutorialManager1;
        private CustomerManager _customerManager;
        private RecipeBookManager _recipeBookManager;
        private TimerManager _timerManager;
        private DialogueManager _dialogueManager;

        public CanvasGroup barContainer;
                
        void Start()
        {
            _tutorialManager1 = GetComponent<TutorialManager1>();
            _customerManager = GetComponent<CustomerManager>();
            _recipeBookManager = GetComponent<RecipeBookManager>();
            _timerManager = GetComponent<TimerManager>();
            _dialogueManager = GetComponent<DialogueManager>();
            _customerManager.AttachDialogueManager(_dialogueManager);
            _tutorialManager1.AttachDialogueManager(_dialogueManager);
        
            EventSystemManager.OnDayStart += StartDay;
            EventSystemManager.OnDrunkCustomerLeave += CheckDrunk;
            EventSystemManager.OnCustomersDepleted += EndDay;
        
            barContainer.GetComponent<FadeCanvas>().FadeIn();
            EventSystemManager.OnLoadBarView();
        }

        private void OnDestroy()
        {
            EventSystemManager.OnDayStart -= StartDay;
            EventSystemManager.OnDrunkCustomerLeave -= CheckDrunk;
            EventSystemManager.OnCustomersDepleted -= EndDay;
        }

        public void StartDay()
        {
            if (Day.CurrentDay == 1)
            {
                _tutorialManager1.StartTutorial();
            }
            else
            {
                _customerManager.StartDay();
                _timerManager.startTimer();
            }
        }

        private void EndDay()
        {
            barContainer.GetComponent<FadeCanvas>().FadeOut();
            StartCoroutine(WaitThenEndDay());
        }

        private IEnumerator WaitThenEndDay()
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("EndOfDay");
        }

        private void CheckDrunk()
        {
            if (Day.DrunkCustomers++ >= Day.MaxDrunkCustomers) EventSystemManager.OnBlitzCalled();
        }

        public void LossByBlitz()
        {
            // todo: display actual loss screen
            print("Loss By Blitz");
        }

        public void BackToMainMenu()
        {
            barContainer.GetComponent<FadeCanvas>().FadeOut();
            StartCoroutine(WaitBeforeMenu());
        }
        
        private IEnumerator WaitBeforeMenu()
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("MainMenu");
        }

        public void QuitGame()
        {
            Technical.QuitGame.Quit();
        }
    }
}
