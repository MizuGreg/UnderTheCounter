using System.Collections;
using SavedGameData;
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
        private TimerManager _timerManager;
        private DialogueManager _dialogueManager;

        public CanvasGroup barContainer;

        [SerializeField] public int forceDay;
                
        void Start()
        {
            _tutorialManager1 = GetComponent<TutorialManager1>();
            _customerManager = GetComponent<CustomerManager>();
            _timerManager = GetComponent<TimerManager>();
            _dialogueManager = GetComponent<DialogueManager>();
            _customerManager.AttachDialogueManager(_dialogueManager);
            if (_tutorialManager1 != null) _tutorialManager1.AttachDialogueManager(_dialogueManager);
        
            EventSystemManager.OnDayStart += StartDay;
            EventSystemManager.OnDrunkCustomer += CheckBlitzWarning;
            EventSystemManager.OnCustomerLeft += CheckDrunk;
            // EventSystemManager.OnBlitzEnd += NextCustomer;
            EventSystemManager.OnCustomersDepleted += EndDay;
            EventSystemManager.OnTutorial1End += EndDay;
            EventSystemManager.OnTrinketObtained += UpdateTrinkets;
            EventSystemManager.OnPosterObtained += UnlockPoster;
        
            barContainer.GetComponent<FadeCanvas>().FadeIn();
            EventSystemManager.OnLoadBarView();
            StartCoroutine(WaitAndStartDay());
        }

        private void OnDestroy()
        {
            EventSystemManager.OnDayStart -= StartDay;
            EventSystemManager.OnDrunkCustomer -= CheckBlitzWarning;
            EventSystemManager.OnCustomerLeft -= CheckDrunk;
            // EventSystemManager.OnBlitzEnd -= NextCustomer;
            EventSystemManager.OnCustomersDepleted -= EndDay;
            EventSystemManager.OnTutorial1End -= EndDay;
        }

        private void PosterEffects()
        {
            if (GameData.IsPosterActive(0))
            {
                GameData.DailyTime += 30;
            }
        }

        private IEnumerator WaitAndStartDay()
        {
            yield return new WaitForSeconds(0.5f);
            StartDay();
        }
        
        public void StartDay()
        {
            #if UNITY_EDITOR
            if (forceDay > 0) GameData.CurrentDay = forceDay;
            #endif
            
            GameData.StartDay();
            PosterEffects();
            _timerManager.SetTime();
            
            if (GameData.CurrentDay == 1)
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
            if (GameData.DrunkCustomers == GameData.MaxDrunkCustomers - 1)
            {
                EventSystemManager.OnBlitzCallWarning();
            }
        }

        private void CheckDrunk()
        {
            if (GameData.DrunkCustomers >= GameData.MaxDrunkCustomers)
            {
                EventSystemManager.OnBlitzCalled();
                GameData.DrunkCustomers = 0;
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

        private void UpdateTrinkets(int trinketID)
        {
            GameData.Trinkets.Add(trinketID);
            DisplayTrinkets();
        }

        private void DisplayTrinkets()
        {
            // TODO: iterate over list of game objects and show the obtained Trinkets
            foreach (int trinketID in GameData.Trinkets)
            {
                print($"Displaying trinket with ID {trinketID}");
            }
        }

        private void UnlockPoster(int posterID)
        {
            GameData.UnlockPoster(posterID);
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
