using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CocktailCreation;
using Newtonsoft.Json;
using SavedGameData;
using Technical;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Bar
{
    public class CustomerManager : MonoBehaviour
    {
        private List<Customer> _dailyCustomers;
        private List<Customer> _dailyBlitzDialogues;
        private Customer _currentCustomer;
        private string _customerName;
        private Image _currentImage;
        private DialogueManager _dialogueManager;

        [SerializeField] private Button choiceButton1;
        [SerializeField] private Button choiceButton2;

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
        private float flatEarning = 5f;

        private void Start()
        {
            EventSystemManager.OnTimeUp += TimeoutCustomers;
            EventSystemManager.OnCocktailMade += ServeCustomer;
            EventSystemManager.OnCustomerLeave += FarewellCustomer;
            EventSystemManager.OnPreparationStart += StartPreparation;
            EventSystemManager.OnMinigameEnd += TriggerHowardDialogue;
            EventSystemManager.MultipleChoiceStart += ShowChoiceButtons;
        
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
            EventSystemManager.OnMinigameEnd -= TriggerHowardDialogue;
            EventSystemManager.MultipleChoiceStart -= ShowChoiceButtons;
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

            // wait a bit more to avoid race conditions
            yield return new WaitForSeconds(0.5f);
            LoadDailyBlitzDialogues(GameData.CurrentDay);
            GreetCustomer();
        }

        private void LoadDailyCustomers(int currentDay)
        {
            // read DailyCustomers json and create daily customers list
            string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/DayData/Day" + currentDay + ".json");
            _dailyCustomers = JsonConvert.DeserializeObject<CustomerList>(jsonString).customers;
            HandleConditionalCustomers();
            
        }

        private void LoadDailyBlitzDialogues(int currentDay)
        {
            // read DailyBlitz json and create daily blitz list
            int blitzDay = currentDay - 1; // blitzes are always TWO days before, this is used for debugging
            string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/BlitzData/Blitz" + blitzDay + ".json");
            _dailyBlitzDialogues = JsonConvert.DeserializeObject<CustomerList>(jsonString).customers;
        }

        private void TriggerHowardDialogue()
        {
            StartCoroutine(StartHowardDialogue());
        }

        private IEnumerator StartHowardDialogue()
        {
            yield return new WaitForSeconds(1.5f);
            _currentImage.sprite = GetSpriteFromCustomerType(CustomerType.Howard);
            customerCanvas.GetComponent<FadeCanvas>().FadeIn();

            yield return new WaitForSeconds(timeBeforeDialogue);
            Dialogue dialogue = new Dialogue("Inspector", _dailyBlitzDialogues[0].lines["greet"]);
            _dialogueManager.StartDialogue(dialogue, DialogueType.Blitz);

            //_dailyBlitzDialogues.RemoveAt(0);
        }

        private void ShowChoiceButtons() 
        {
            choiceButton1.gameObject.SetActive(true);
            choiceButton2.gameObject.SetActive(true);

            choiceButton1.GetComponentInChildren<TextMeshProUGUI>().text = _dailyBlitzDialogues[0].lines["choices"][0];
            choiceButton2.GetComponentInChildren<TextMeshProUGUI>().text = _dailyBlitzDialogues[0].lines["choices"][1];

            choiceButton1.onClick.AddListener(() => OnChoiceSelected(0));
            choiceButton2.onClick.AddListener(() => OnChoiceSelected(1));
        }

        private void OnChoiceSelected(int choiceIndex)
        {
            Debug.Log("Choice selected: " + choiceIndex);

            choiceButton1.gameObject.SetActive(false);
            choiceButton2.gameObject.SetActive(false);

            Dialogue correctDialogue = new Dialogue("Inspector", _dailyBlitzDialogues[0].lines["correct"]);
            Dialogue wrongDialogue = new Dialogue("Inspector", _dailyBlitzDialogues[0].lines["wrong"]);

            if (choiceIndex == 0)
            {
                _dialogueManager.StartDialogue(correctDialogue, DialogueType.NoDrink);
            }
            else
            {
                _dialogueManager.StartDialogue(wrongDialogue, DialogueType.NoDrink);
            }

            EventSystemManager.OnBlitzEnd();
            _dailyBlitzDialogues.RemoveAt(0);
        }

        // Handles customers that appear or not depending on some conditions, or that have specific sprites depending on conditions
        private void HandleConditionalCustomers()
        {
            switch (GameData.CurrentDay)
            {
                case 5:
                    if (GameData.Choices["MargaretDrunk"])
                    {
                        _dailyCustomers = _dailyCustomers.Where(customer => customer.sprite != CustomerType.Margaret).ToList(); // leave out normal Margaret
                    }
                    else
                    {
                        _dailyCustomers = _dailyCustomers.Where(customer => customer.sprite != CustomerType.MargaretDrunk).ToList(); // leave out drunk Margaret
                    }
                    break;
                case 6:
                    if (GameData.Choices["MafiaDeal"])
                    {
                        // show goon
                    }
                    else
                    {
                        _dailyCustomers = _dailyCustomers.Where(customer => customer.sprite != CustomerType.MafiaGoon).ToList(); // leave out goon
                    }
                    break;
                default:
                    break;
            }
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
                _currentCustomer.sprite is CustomerType.Howard or CustomerType.MafiaGoon or CustomerType.ErnestUnion
                    ? DialogueType.NoDrink
                    : DialogueType.Greet);
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
