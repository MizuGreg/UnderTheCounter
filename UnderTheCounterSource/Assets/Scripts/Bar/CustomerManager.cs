using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        
        [Range (0.0f, 3f)]
        public float timeBetweenCustomers = 2.5f;
        [Range(0.0f, 3f)]
        public float timeBeforeDialogue = 1f;
        [Range(0.0f, 3f)]
        public float timeBeforeFadeout = 1f;

        private void Start()
        {
            EventSystemManager.OnTimeUp += TimeoutCustomers;
            EventSystemManager.OnCocktailMade += ServeCustomer;
            EventSystemManager.OnCustomerLeave += FarewellCustomer;
        
            _currentImage = customerCanvas.transform.Find("CustomerSprite").gameObject.GetComponent<Image>();
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
            string jsonString = File.ReadAllText("Assets/Data/CustomerData/Day" + currentDay + ".json");
            // dailyCustomers = JsonUtility.FromJson<CustomerList>(jsonString).customers;
            
            _dailyCustomers = JsonConvert.DeserializeObject<CustomerList>(jsonString).customers;
            
        }
        
        private IEnumerator WaitAndGreetDialogue()
        {
            _dialogueManager.StartDialogue(
                new Dialogue(_customerName, _currentCustomer.lines["greet"]),
                DialogueType.Greet);
            yield return null;
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
                customerCanvas.GetComponent<CanvasFadeAnimation>().FadeIn();
                StartCoroutine(WaitAndGreetDialogue());
                EventSystemManager.OnCustomerEnter();
            }
            else
            {
                EventSystemManager.OnCustomersDepleted();
            }
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
            customerCanvas.GetComponent<CanvasFadeAnimation>().FadeOut();
            yield return WaitBeforeNextCustomer();
        }

        private IEnumerator WaitBeforeNextCustomer()
        {
            yield return new WaitForSeconds(timeBetweenCustomers);
            GreetCustomer();
        }

        private void ServeCustomer(Cocktail cocktail)
        {
            // we compare with current customer's cocktail, call dialogue line in dialogue manager accordingly
            CocktailType order = _currentCustomer.order;
            Dialogue dialogue;
            if (cocktail.type != order)
            {
                dialogue = new Dialogue(_customerName, _currentCustomer.lines["leaveWrong"]);
            }
            else if (cocktail.wateredDown)
            {
                dialogue = new Dialogue(_customerName, _currentCustomer.lines["leaveWater"]);
            }
            else
            {
                dialogue = new Dialogue(_customerName, _currentCustomer.lines["leaveCorrect"]);
            }
            _dialogueManager.StartDialogue(dialogue, DialogueType.Leave);
            
            Day.TodayEarnings += 5 + _currentCustomer.tip;
            print($"Current earnings: {Day.TodayEarnings}");
            
            // if not watered down, we throw onDrunkCustomer event
            if (!cocktail.wateredDown) EventSystemManager.OnDrunkCustomerLeave();
        }
    

        public void PlayTutorial()
        {
            // todo: tutorial with ex bar owner. for now it's just a simple customer, but it could become something more
            // complex in the future if need be
            GreetCustomer();
        }
    }
}
