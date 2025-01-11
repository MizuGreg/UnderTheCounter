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
        [Header("Canvases")]
        public CanvasGroup barContainer;
        [SerializeField] private CanvasGroup trinketContainer;
        
        private TutorialManager1 _tutorialManager1;
        private CustomerManager _customerManager;
        private TimerManager _timerManager;
        private DialogueManager _dialogueManager;

        [Header("Debug fields")]
        [SerializeField] private int forceDay;
        [SerializeField] private bool didBlitzHappen;
                
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
            EventSystemManager.OnBlitzCalled += BlitzHappened;
            EventSystemManager.OnCustomersDepleted += EndDay;
            EventSystemManager.OnTutorial1End += EndDay;
            EventSystemManager.OnTrinketObtained += UpdateTrinkets;
            EventSystemManager.OnPosterObtained += UnlockPoster;
        
            barContainer.GetComponent<FadeCanvas>().FadeIn();
            EventSystemManager.OnLoadBarView();
            StartCoroutine(WaitAndStartDay());
            DisplayTrinkets();
        }

        private void OnDestroy()
        {
            EventSystemManager.OnDayStart -= StartDay;
            EventSystemManager.OnDrunkCustomer -= CheckBlitzWarning;
            EventSystemManager.OnCustomerLeft -= CheckDrunk;
            EventSystemManager.OnBlitzCalled -= BlitzHappened;
            EventSystemManager.OnCustomersDepleted -= EndDay;
            EventSystemManager.OnTutorial1End -= EndDay;
            EventSystemManager.OnTrinketObtained -= UpdateTrinkets;
            EventSystemManager.OnPosterObtained -= UnlockPoster;
        }

        private void PosterEffects()
        {
            if (GameData.IsPosterActive(1))
            {
                GameData.DailyTime += 40;
            }

            if (GameData.IsPosterActive(3))
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

            if (GameData.CurrentDay == 3)
            { // unlocks posters that have effects that influence the blitz (otherwise reading them would spoil the mechanic)
                UnlockPoster(4);
                UnlockPoster(6);
            }
        }

        private void StartTimer()
        {
            _timerManager.StartTimer();
        }

        private void EndDay()
        {
            if (!didBlitzHappen) // clean day, counts as two successful blitzes
            {
                GameData.BlitzSuccessful();
                GameData.BlitzSuccessful();
            }
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
            if (GameData.Trinkets.Count == 3)
            {
                EventSystemManager.OnHalfTrinketCollected();
            }
            else if (GameData.Trinkets.Count == 6)
            {
                EventSystemManager.OnAllTrinketCollected();
            }
            DisplayTrinkets();
        }

        private void DisplayTrinkets()
        {
            // TODO: iterate over list of game objects and show the obtained Trinkets
            foreach (int trinketID in GameData.Trinkets)
            {
                print($"Displaying trinket with ID {trinketID}");
                trinketContainer.transform.GetChild(trinketID).gameObject.SetActive(true);
            }
        }

        private void UnlockPoster(int posterID)
        {
            GameData.UnlockPoster(posterID);
        }
        
        public void UpdateBarName(string name)
        {
            GameData.BarName = name is null or "" ? "The Chitchat" : name;
        }
        
        private void BlitzHappened()
        {
            didBlitzHappen = true;
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
        
        private IEnumerator LoadLoseScreen()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("GameOverScreen");
        }

        public void QuitGame()
        {
            Technical.QuitGame.Quit();
        }
    }
}
