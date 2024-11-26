using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CocktailCreation;
using Newtonsoft.Json;
using Technical;
using UnityEngine;
using UnityEngine.UI;

namespace Bar
{
    public class CustomerManager : MonoBehaviour
    {
        private List<Customer> _dailyCustomers;
        private Customer _currentCustomer;
        private string _customerName;
        private Image _currentImage;
        private DialogueManager _dialogueManager;

        public CanvasGroup customerCanvas;
        
        [SerializeField] private PricePopup pricePopup;
        
        [Range (0.1f, 5f)]
        public float timeBetweenCustomers = 2.5f;
        [Range(0.1f, 5f)]
        public float timeBeforeDialogue = 1f;
        [Range(0.0f, 3f)]
        public float timeBeforeFadeout = 1f;

        private void Start()
        {
            EventSystemManager.OnTimeUp += TimeoutCustomers;
            EventSystemManager.OnCocktailMade += ServeCustomer;
            EventSystemManager.OnCustomerLeave += FarewellCustomer;
        
            _currentImage = customerCanvas.transform.Find("CustomerSprite").gameObject.GetComponent<Image>();
            pricePopup.gameObject.SetActive(true);
            
            #if !UNITY_EDITOR
            GameObject.FindWithTag("Debug").SetActive(true);
            #endif
            
        }

        public void AttachDialogueManager(DialogueManager dialogueManager)
        {
            this._dialogueManager = dialogueManager;
        }

        private void TimeoutCustomers()
        {
            _dailyCustomers.Clear(); // depletes daily customers
        }

        public void StartDay()
        {
            LoadDailyCustomers(Day.CurrentDay);
            if (Day.CurrentDay == 1) PlayTutorial();
            else GreetCustomer();
        }

        private void LoadDailyCustomers(int currentDay)
        {
            // read DailyCustomers json and create daily customers list
            #if UNITY_EDITOR
            string jsonString = File.ReadAllText("Assets/Data/DayData/Day" + currentDay + ".json");
            #else
            string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/DayData/Day" + currentDay + ".json");
            #endif
            
            _dailyCustomers = JsonConvert.DeserializeObject<CustomerList>(jsonString).customers;
            
        }

        public void GreetCustomer()
        {
            // pick the first customer from the list and set it to current. if empty we throw onCustomersDepleted
            // otherwise send out lines to display to the dialogue manager
            if (_dailyCustomers.Count > 0)
            {
                _currentCustomer = _dailyCustomers[0];
                _customerName = _currentCustomer.sprite.ToString();
                _dailyCustomers.RemoveAt(0);
            
                _currentImage.sprite = GetSpriteFromCustomerType(_currentCustomer.sprite);
                customerCanvas.GetComponent<FadeCanvas>().FadeIn();
                StartCoroutine(WaitAndGreetDialogue());
                EventSystemManager.OnCustomerEnter();
            }
            else
            {
                EventSystemManager.OnCustomersDepleted();
            }
        }
        
        private IEnumerator WaitAndGreetDialogue()
        {
            yield return new WaitForSeconds(timeBeforeDialogue);
            _dialogueManager.StartDialogue(
                new Dialogue(_customerName, _currentCustomer.lines["greet"]),
                DialogueType.Greet);
        }

        private Sprite GetSpriteFromCustomerType(CustomerType customerType)
        {
            // assumes that the file name is just the same as the customer type, for now
            try
            {
                return Resources.Load("Sprites/Characters/" + customerType, typeof(Sprite)) as Sprite;
            }
            catch (Exception e)
            {
                print($"Exception in getSprite: {e}");
                return Resources.Load("Sprites/Characters/Margaret", typeof(Sprite)) as Sprite;
            }
        
        }

        private void FarewellCustomer()
        {
            StartCoroutine(WaitBeforeFadeOut());
        }

        private IEnumerator WaitBeforeFadeOut()
        {
            yield return new WaitForSeconds(timeBeforeFadeout);
            customerCanvas.GetComponent<FadeCanvas>().FadeOut();
            yield return WaitBeforeNextCustomer();
        }

        private IEnumerator WaitBeforeNextCustomer()
        {
            yield return new WaitForSeconds(timeBetweenCustomers);
            GreetCustomer();
        }

        private void ServeCustomer(Cocktail cocktail)
        {
            // referenziare la lista degli ingredienti per controllare correttezza
            // we compare with current customer's cocktail, call dialogue line in dialogue manager accordingly
            CocktailType order = _currentCustomer.order;
            Dialogue dialogue;
            float earning;
            if (cocktail.type != order)
            {
                dialogue = new Dialogue(_customerName, _currentCustomer.lines["leaveWrong"]);
                earning = 5 + _currentCustomer.tip / 5;
            }
            else if (cocktail.isWatered)
            {
                dialogue = new Dialogue(_customerName, _currentCustomer.lines["leaveWater"]);
                earning = 5 + _currentCustomer.tip / 4;
            }
            else
            {
                dialogue = new Dialogue(_customerName, _currentCustomer.lines["leaveCorrect"]);
                earning = 5 + _currentCustomer.tip;
            }
            _dialogueManager.StartDialogue(dialogue, DialogueType.Leave);

            pricePopup.DisplayPrice(earning);
            Day.TodayEarnings += earning;
            print($"Current earnings: {Day.TodayEarnings}");
            
            // if not watered down, we throw onDrunkCustomer event
            if (!cocktail.isWatered) EventSystemManager.OnDrunkCustomerLeave();
        }
    

        public void PlayTutorial()
        {
            // todo: tutorial with ex bar owner. for now it's just a simple customer, but it could become something more
            // complex in the future if need be
            GreetCustomer();
        }
    }
}
