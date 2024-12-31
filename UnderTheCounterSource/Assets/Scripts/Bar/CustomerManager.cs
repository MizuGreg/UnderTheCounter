using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using CocktailCreation;
using Newtonsoft.Json;
using SavedGameData;
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

        private float earningMultiplier = 1f;
        private float flatEarning = 4f;

        private void Start()
        {
            EventSystemManager.OnTimeUp += TimeoutCustomers;
            EventSystemManager.OnCocktailMade += ServeCustomer;
            EventSystemManager.OnCustomerLeave += FarewellCustomer;
            EventSystemManager.OnPreparationStart += StartPreparation;
        
            _currentImage = customerCanvas.transform.Find("CustomerSprite").gameObject.GetComponent<Image>();
            pricePopup.gameObject.SetActive(true);

            PosterEffects();
        }

        private void OnDestroy()
        {
            EventSystemManager.OnTimeUp -= TimeoutCustomers;
            EventSystemManager.OnCocktailMade -= ServeCustomer;
            EventSystemManager.OnCustomerLeave -= FarewellCustomer;
            EventSystemManager.OnPreparationStart -= StartPreparation;
        }

        public void AttachDialogueManager(DialogueManager dialogueManager)
        {
            this._dialogueManager = dialogueManager;
        }
        
        private void PosterEffects()
        {
            if (GameData.IsPosterActive(1))
            {
                earningMultiplier = 1.3f;
            }

            if (GameData.IsPosterActive(2))
            {
                flatEarning += 1;
            }
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
            yield return new WaitForSeconds(1.5f);
            LoadDailyCustomers(GameData.CurrentDay);
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
                if (_currentCustomer.sprite == CustomerType.Howard) _customerName = "Inspector";
                else _customerName = _currentCustomer.sprite.ToString();
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
                if (_currentCustomer.orderOnPostIt != null)
                {
                    EventSystemManager.OnWritePostIt(_currentCustomer.orderOnPostIt);
                }
                else
                {
                    string orderOnPostIt = Regex.Replace(_currentCustomer.order.ToString(),
                        @"([a-z])([A-Z])", "$1 $2"); // adds a space between words
                    EventSystemManager.OnWritePostIt(orderOnPostIt);
                }
                EventSystemManager.OnMakeCocktail(_currentCustomer.order);
            }
            else // special flow, for calling start preparation in a "shortcut" way
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
            if (customerCocktail.gameObject.activeSelf) customerCocktail.GetComponent<FadeCanvas>().FadeOut();
            customerCanvas.GetComponent<FadeCanvas>().FadeOut();
            EventSystemManager.OnCustomerLeft();
        }

        public IEnumerator WaitBeforeNextCustomer()
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
                float earning = flatEarning;
                
                if (cocktail.type == order) // correct cocktail through and through
                {
                    if (cocktail.isWatered)
                    {
                        dialogue = new Dialogue(_customerName, _currentCustomer.lines["water"]);
                        earning += _currentCustomer.tip / 3;
                    }
                    else
                    {
                        dialogue = new Dialogue(_customerName, _currentCustomer.lines["correct"]);
                        earning += _currentCustomer.tip;
                    }
                }
                
                else if (cocktail.type == CocktailType.Wrong) // completely wrong cocktail, a mess
                {
                    dialogue = new Dialogue(_customerName, _currentCustomer.lines["wrong"]);
                    earning = 0;
                }
                
                else // correctly executed cocktail, but not the one the customer ordered
                {
                    if (_currentCustomer.lines.ContainsKey(cocktail.type.ToString())) // if we have a custom line for that cocktail
                    {
                        dialogue = new Dialogue(_customerName, _currentCustomer.lines[cocktail.type.ToString()]);
                        earning += _currentCustomer.tip / 3;
                    }
                    else if (_currentCustomer.lines.ContainsKey("incorrect")) // if we have a generic line for an incorrect but well-done cocktail
                    {
                        dialogue = new Dialogue(_customerName, _currentCustomer.lines["incorrect"]);
                        earning += _currentCustomer.tip / 3;
                    }
                    else // fallback to standard "wrong cocktail" line
                    {
                        dialogue = new Dialogue(_customerName, _currentCustomer.lines["wrong"]);
                        earning = 0;
                    }
                }

                earning = Mathf.Round(earning * earningMultiplier);
                pricePopup.DisplayPrice(earning);
                _dialogueManager.StartDialogue(dialogue, DialogueType.Leave);

                GameData.TodayEarnings += earning;
            
                // if not watered down and not a complete mess cocktail, we increase the drunk customers counter
                if (!cocktail.isWatered && cocktail.type != CocktailType.Wrong)
                {
                    GameData.DrunkCustomers++;
                    EventSystemManager.OnDrunkCustomer();
                }
            }
            else
            {
                // The control flow enters here in case of tutorial
                
                customerCocktail.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/Cocktails/Ripple/Ripple_tot");
                customerCocktail.GetComponent<FadeCanvas>().FadeIn();

                float earning = 10;
                pricePopup.DisplayPrice(earning);
                GameData.TodayEarnings += earning;
            }
            
        }
    }
}
