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

        [SerializeField] public int forceDay = 2;
                
        void Start()
        {
            _tutorialManager1 = GetComponent<TutorialManager1>();
            _customerManager = GetComponent<CustomerManager>();
            _recipeBookManager = GetComponent<RecipeBookManager>();
            _timerManager = GetComponent<TimerManager>();
            _dialogueManager = GetComponent<DialogueManager>();
            _customerManager.AttachDialogueManager(_dialogueManager);
            if (_tutorialManager1 != null) _tutorialManager1.AttachDialogueManager(_dialogueManager);
        
            EventSystemManager.OnDayStart += StartDay;
            EventSystemManager.OnDrunkCustomerLeave += CheckBlitzWarning;
            EventSystemManager.OnCustomerLeft += CheckDrunk;
            EventSystemManager.OnBlitzEnd += NextCustomer;
            EventSystemManager.OnCustomersDepleted += EndDay;
            EventSystemManager.OnTutorial1End += EndDay;
        
            barContainer.GetComponent<FadeCanvas>().FadeIn();
            EventSystemManager.OnLoadBarView();
            
            StartCoroutine(WaitAndStartDay());
        }

        private void OnDestroy()
        {
            EventSystemManager.OnDayStart -= StartDay;
            EventSystemManager.OnDrunkCustomerLeave -= CheckBlitzWarning;
            EventSystemManager.OnCustomerLeft -= CheckDrunk;
            EventSystemManager.OnBlitzEnd -= NextCustomer;
            EventSystemManager.OnCustomersDepleted -= EndDay;
            EventSystemManager.OnTutorial1End -= EndDay;
        }

        private void PosterEffects()
        {
            if (Day.IsPosterActive(0))
            {
                Day.DailyTime += 30;
            }
        }

        private IEnumerator WaitAndStartDay()
        {
            yield return new WaitForSeconds(1f);
            StartDay();
        }
        
        public void StartDay()
        {
            #if UNITY_EDITOR
            if (forceDay != 0) Day.CurrentDay = forceDay;
            #endif
            
            Day.StartDay();
            PosterEffects();
            _timerManager.SetTime();
            
            if (Day.CurrentDay == 1)
            {
                _tutorialManager1.StartTutorial();
            }
            else
            {
                _customerManager.StartDay();
                StartTimer();
            }
        }

        private void StartTimer()
        {
            _timerManager.StartTimer();
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

        private void CheckBlitzWarning()
        {
            if (Day.DrunkCustomers == Day.MaxDrunkCustomers - 1)
            {
                EventSystemManager.OnBlitzCallWarning();
            }
        }

        private void CheckDrunk()
        {
            if (Day.DrunkCustomers >= Day.MaxDrunkCustomers)
            {
                EventSystemManager.OnBlitzCalled();
                Day.DrunkCustomers = 0;
            }
            else 
            {
                NextCustomer();
            }
        }

        private void NextCustomer()
        {
            StartCoroutine(_customerManager.WaitBeforeNextCustomer());
        }

        public void BackToMainMenu()
        {
            barContainer.GetComponent<FadeCanvas>().FadeOut();
            StartCoroutine(WaitBeforeMenu());
        }
        
        private IEnumerator WaitBeforeMenu()
        {
            yield return new WaitForSeconds(1.1f);
            SceneManager.LoadScene("MainMenu");
        }

        public void QuitGame()
        {
            Technical.QuitGame.Quit();
        }
    }
}
