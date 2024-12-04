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

        [SerializeField] private CanvasGroup customerCanvas;
        
        [SerializeField] private PricePopup pricePopup;

        [SerializeField] private GameObject customerCocktail;
        
        [Range (0.1f, 5f)]
        public float timeBetweenCustomers = 2.5f;
        [Range(0.1f, 5f)]
        public float timeBeforeDialogue = 1f;
        [Range(0.0f, 3f)]
        public float timeBeforeFadeout = 1f;

        private float earningMultiplier;

        private void Start()
        {
            EventSystemManager.OnTutorial1End += StartDay;
            EventSystemManager.OnTimeUp += TimeoutCustomers;
            EventSystemManager.OnCocktailMade += ServeCustomer;
            EventSystemManager.OnCustomerLeave += FarewellCustomer;
            EventSystemManager.OnPreparationStart += StartPreparation;
        
            _currentImage = customerCanvas.transform.Find("CustomerSprite").gameObject.GetComponent<Image>();
            pricePopup.gameObject.SetActive(true);
            
            #if !UNITY_EDITOR
            GameObject.FindWithTag("Debug").SetActive(true);
            #endif

            PosterEffects();

        }

        private void PosterEffects()
        {
            if (Day.IsPosterActive(1))
            {
                earningMultiplier = 1.5f;
            }
            else
            {
                earningMultiplier = 1f;
            }
        }

        private void OnDestroy()
        {
            EventSystemManager.OnTutorial1End -= FarewellCustomer;
            EventSystemManager.OnTimeUp -= TimeoutCustomers;
            EventSystemManager.OnCocktailMade -= ServeCustomer;
            EventSystemManager.OnCustomerLeave -= FarewellCustomer;
            EventSystemManager.OnPreparationStart -= StartPreparation;
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
            StartCoroutine(WaitAndStartDay());
        }

        private IEnumerator WaitAndStartDay()
        {
            yield return new WaitForSeconds(2f);
            LoadDailyCustomers(Day.CurrentDay);
            GreetCustomer();
        }

        private void LoadDailyCustomers(int currentDay)
        {
            // read DailyCustomers json and create daily customers list
            string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/DayData/Day" + currentDay + ".json");
            
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
                _currentCustomer.sprite == CustomerType.Howard ? DialogueType.Inspector : DialogueType.Greet);
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

        private void StartPreparation() {
            if (_currentCustomer != null)
            {
                EventSystemManager.OnMakeCocktail(_currentCustomer.order);
            }
            else
            {
                EventSystemManager.OnMakeCocktail(CocktailType.Wrong);
            }
        }

        private void FarewellCustomer()
        {
            StartCoroutine(WaitBeforeFadeOut());
        }

        private IEnumerator WaitBeforeFadeOut()
        {
            yield return new WaitForSeconds(timeBeforeFadeout);
            customerCocktail.GetComponent<FadeCanvas>().FadeOut();
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
            if (_currentCustomer != null)
            {
                CocktailType order = _currentCustomer.order;
                customerCocktail.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/Cocktails/{cocktail.type}/{cocktail.type}_tot");
                customerCocktail.GetComponent<FadeCanvas>().FadeIn();
            
                // we compare with current customer's cocktail, call dialogue line in dialogue manager accordingly
                Dialogue dialogue;
                float earning = 5 * earningMultiplier;
                if (cocktail.type != order)
                {
                    dialogue = new Dialogue(_customerName, _currentCustomer.lines["leaveWrong"]);
                    earning += 0;
                }
                else if (cocktail.isWatered)
                {
                    dialogue = new Dialogue(_customerName, _currentCustomer.lines["leaveWater"]);
                    earning += _currentCustomer.tip / 4;
                }
                else
                {
                    dialogue = new Dialogue(_customerName, _currentCustomer.lines["leaveCorrect"]);
                    earning += _currentCustomer.tip;
                }

                pricePopup.DisplayPrice(earning);
                _dialogueManager.StartDialogue(dialogue, DialogueType.Leave);

                Day.TodayEarnings += earning;
            
                // if not watered down, we throw onDrunkCustomer event
                if (!cocktail.isWatered) EventSystemManager.OnDrunkCustomerLeave();
            }
            else
            {
                // The control flow enters here in case of tutorial
                
                customerCocktail.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/Cocktails/Ripple/Ripple_tot");
                customerCocktail.GetComponent<FadeCanvas>().FadeIn();

                float earning = 10;
                pricePopup.DisplayPrice(earning);
                Day.TodayEarnings += earning;
            }
            
        }
    }
}
