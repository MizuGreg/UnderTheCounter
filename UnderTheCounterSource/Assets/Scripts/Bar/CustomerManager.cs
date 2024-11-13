using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
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
        [Range (0.1f, 3f)]
        public float timeBetweenCustomers = 2.5f;

        [Range(0.1f, 3f)]
        public float timeBeforeDialogue = 1f;

        void Start()
        {
            LoadDailyCustomers(DaySO.currentDay);
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

        private void LoadDailyCustomers(int currentDay)
        {
            // read DailyCustomers json and create daily customers list
            string jsonString = File.ReadAllText("Assets/Data/CustomerData/Day" + currentDay + ".json");
            // dailyCustomers = JsonUtility.FromJson<CustomerList>(jsonString).customers;
            
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
                customerCanvas.GetComponent<CanvasFadeAnimation>().FadeIn();
                StartCoroutine(WaitAndGreet());
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

        private IEnumerator WaitAndGreet()
        {
            yield return new WaitForSeconds(timeBeforeDialogue);
            _dialogueManager.StartDialogue(new Dialogue(
                _customerName,
                _currentCustomer.lines["greet"]));
        }

        private void FarewellCustomer()
        {
            // fade out customer, wait a bit, then call greetCustomer for next customer
            customerCanvas.GetComponent<CanvasFadeAnimation>().FadeOut();
            EventSystemManager.OnCustomerLeave();
            StartCoroutine(WaitBeforeNextCustomer());
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
            if (cocktail.type != order)
            {
                _dialogueManager.StartDialogue(new Dialogue(_customerName, _currentCustomer.lines["leaveWrong"]));
            }
            else if (cocktail.wateredDown)
            {
                // todo: dialogueManager.customerServe(currentCustomer.lines["leaveWater"]);
            }
            else
            {
                // todo: dialogueManager.customerServe(currentCustomer.lines["leaveCorrect"]);
            }
            
            DaySO.todayEarnings += 5 + _currentCustomer.tip;
            print($"Current earnings: {DaySO.todayEarnings}");
            
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
