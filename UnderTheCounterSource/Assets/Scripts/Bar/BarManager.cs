using System.Collections;
using Technical;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bar
{
    public class BarManager : MonoBehaviour
    {
        private CustomerManager _customerManager;
        private RecipeBookManager _recipeBookManager;
        private TimerManager _timerManager;
        private DialogueManager _dialogueManager;

        public CanvasGroup barContainer;
        public CanvasGroup barCanvas;
        
        [SerializeField] private CanvasGroup timerCanvas;
        
        void Start()
        {
            _customerManager = GetComponent<CustomerManager>();
            _recipeBookManager = GetComponent<RecipeBookManager>();
            _timerManager = GetComponent<TimerManager>();
            _dialogueManager = GetComponent<DialogueManager>();
            _customerManager.AttachDialogueManager(_dialogueManager);
        
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
            _customerManager.StartDay();
            _timerManager.startTimer();
        
        }

        private void EndDay()
        {
            barCanvas.GetComponent<FadeCanvas>().FadeOut();
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
