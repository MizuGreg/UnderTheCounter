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
using Extra;

namespace Bar
{
    public class CustomerManager : MonoBehaviour
    {
        private List<Customer> _dailyCustomers;
        private Customer _currentCustomer;
        private string _customerName;
        private Image _currentImage;
        private DialogueManager _dialogueManager;

        [Header("Blitz-related fields")]
        private bool isBlitzHappening;
        private List<BlitzDialogue> _dailyBlitzDialogues;
        private BlitzDialogue currentBlitzDialogue;
        [SerializeField] private Button choiceButton1;
        [SerializeField] private Button choiceButton2;

        [Header("Customer-related fields")]
        [SerializeField] private CanvasGroup customerCanvas;
        [SerializeField] private PricePopup pricePopup;
        [SerializeField] private GameObject customerCocktail;
        
        [Header("Timing-related variables")]
        [Range (0.1f, 5f)]
        public float timeBetweenCustomers = 2.5f;
        [Range(0.1f, 5f)]
        public float timeBeforeDialogue = 1f;
        [Range(0.0f, 3f)]
        public float timeBeforeFadeout = 1f;

        private float earningMultiplier = 1f;
        private float flatEarning = 5f;

        private int _totalDailyCustomers;
        private int _servedCustomers;

        private int _trinketToDisplay = -1;
        private int _posterToDisplay = -1;

        private void Start()
        {
            EventSystemManager.OnTimeUp += TimeoutCustomers;
            EventSystemManager.OnCocktailMade += ServeCustomer;
            EventSystemManager.OnCustomerLeave += FarewellCustomer;
            EventSystemManager.OnPreparationStart += StartPreparation;
            EventSystemManager.OnHowardEnter += TriggerHowardDialogue;
            EventSystemManager.MultipleChoiceStart += ShowChoiceButtons;
            EventSystemManager.OnTrinketCollected += SetTrinketToDisplay;
            EventSystemManager.OnPosterObtained += SetPosterToDisplay;
        
            _currentImage = customerCanvas.transform.Find("CustomerSprite").gameObject.GetComponent<Image>();
            pricePopup.gameObject.SetActive(true);

            _servedCustomers = 0;
        }

        private void OnDestroy()
        {
            EventSystemManager.OnTimeUp -= TimeoutCustomers;
            EventSystemManager.OnCocktailMade -= ServeCustomer;
            EventSystemManager.OnCustomerLeave -= FarewellCustomer;
            EventSystemManager.OnPreparationStart -= StartPreparation;
            EventSystemManager.OnHowardEnter -= TriggerHowardDialogue;
            EventSystemManager.MultipleChoiceStart -= ShowChoiceButtons;
            EventSystemManager.OnTrinketCollected -= SetTrinketToDisplay;
        }

        public void AttachDialogueManager(DialogueManager dialogueManager)
        {
            this._dialogueManager = dialogueManager;
        }
        
        private void PosterEffects()
        {
            if (GameData.IsPosterActive(0))
            {
                earningMultiplier += 0.25f;
                if (_dailyCustomers[^1].sprite is CustomerType.MafiaGoon or CustomerType.Howard or CustomerType.ErnestUnion)
                {
                    _dailyCustomers.RemoveAt(_dailyCustomers.Count - 2); // takes out second-to-last customer
                }
                else
                {
                    _dailyCustomers.RemoveAt(_dailyCustomers.Count - 1); // takes out last customer
                }
            }
            if (GameData.IsPosterActive(1))
            {
                if (_dailyCustomers[^1].sprite is CustomerType.MafiaGoon or CustomerType.Howard or CustomerType.ErnestUnion)
                {
                    _dailyCustomers.RemoveAt(_dailyCustomers.Count - 2); // takes out second-to-last customer
                }
                else
                {
                    _dailyCustomers.RemoveAt(_dailyCustomers.Count - 1); // takes out last customer
                }
            }
            if (GameData.IsPosterActive(3))
            {
                earningMultiplier -= 0.2f;
            }
            if (GameData.IsPosterActive(5))
            {
                earningMultiplier -= 0.2f;
            }
        }

        private void TimeoutCustomers()
        {
            try
            {
                Customer lastCustomer = _dailyCustomers[^1];
                if (lastCustomer.sprite is CustomerType.MafiaGoon or CustomerType.Howard or CustomerType.ErnestUnion)
                {
                    // we keep the "last visit"
                    _dailyCustomers.Clear();
                    _dailyCustomers.Add(lastCustomer);
                }
                else
                {
                    _dailyCustomers.Clear(); // deplete all remaining daily customers
                }
            }
            catch
            {
                // ignore timeout, we've run out of customers
            }
        }

        public void StartDay()
        {
            StartCoroutine(WaitAndStartDay());
        }

        private IEnumerator WaitAndStartDay()
        {
            yield return new WaitForSeconds(1f);
            LoadDailyCustomers(GameData.CurrentDay);
            PosterEffects();
            
            _totalDailyCustomers = _dailyCustomers.Count;

            if (GameData.CurrentDay >= 3)
            {
                LoadDailyBlitzDialogues(GameData.CurrentDay);
            }
            
            GreetCustomer();
        }

        private void LoadDailyCustomers(int day)
        {
            // read DailyCustomers json and create daily customers list
            string jsonString = Resources.Load<TextAsset>($"TextAssets/DayData/Day{day}").text;
            _dailyCustomers = JsonConvert.DeserializeObject<List<Customer>>(jsonString);
            HandleConditionalCustomers();
        }

        private void LoadDailyBlitzDialogues(int day)
        {
            // read DailyBlitz json and create daily blitz list
            string jsonString = Resources.Load<TextAsset>($"TextAssets/BlitzData/Blitz{day}").text;
            _dailyBlitzDialogues = JsonConvert.DeserializeObject<List<BlitzDialogue>>(jsonString);
        }

        private void TriggerHowardDialogue()
        {
            isBlitzHappening = true;
            StartCoroutine(StartHowardDialogue());
        }

        private IEnumerator StartHowardDialogue()
        {
            yield return new WaitForSeconds(1.5f);
            _currentImage.sprite = GetSpriteFromCustomerType(CustomerType.Howard);
            customerCanvas.GetComponent<FadeCanvas>().FadeIn();
            EventSystemManager.OnCustomerEnter();

            yield return new WaitForSeconds(timeBeforeDialogue);
            currentBlitzDialogue = _dailyBlitzDialogues[0];
            Dialogue dialogue = new Dialogue("Inspector", currentBlitzDialogue.lines["greet"]);
            _dialogueManager.StartDialogue(dialogue, DialogueType.MultipleChoice);

            // _dailyBlitzDialogues.RemoveAt(0);
        }

        private void ShowChoiceButtons() 
        {
            choiceButton1.GetComponent<FadeCanvas>().FadeIn();
            choiceButton2.GetComponent<FadeCanvas>().FadeIn();

            if (isBlitzHappening)
            {
                choiceButton1.GetComponentInChildren<TextMeshProUGUI>().text = currentBlitzDialogue.lines["choices"][0];
                choiceButton2.GetComponentInChildren<TextMeshProUGUI>().text = currentBlitzDialogue.lines["choices"][1];
            }
            else
            {
                choiceButton1.GetComponentInChildren<TextMeshProUGUI>().text = _currentCustomer.lines["choices"][0];
                choiceButton2.GetComponentInChildren<TextMeshProUGUI>().text = _currentCustomer.lines["choices"][1];
            }
        }

        public void OnChoiceSelected(int choiceIndex)
        {
            Debug.Log("Choice selected: " + choiceIndex);

            choiceButton1.GetComponent<FadeCanvas>().FadeOut();
            choiceButton2.GetComponent<FadeCanvas>().FadeOut();
            
            if (isBlitzHappening) // we're in a blitz choice
            {
                isBlitzHappening = false;
                if (choiceIndex == currentBlitzDialogue.correctChoice)
                {
                    GameData.BlitzSuccessful();
                    Dialogue correctDialogue = new Dialogue("Inspector", currentBlitzDialogue.lines["correct"]);
                    _dialogueManager.StartDialogue(correctDialogue, DialogueType.Leave);
                }
                else
                {
                    GameData.BlitzFailed();
                    Dialogue wrongDialogue = new Dialogue("Inspector", currentBlitzDialogue.lines["wrong"]);
                    _dialogueManager.StartDialogue(wrongDialogue, DialogueType.Leave);
                }

                EventSystemManager.OnBlitzEnd();
                _dailyBlitzDialogues.RemoveAt(0);
            }
            else // we're in a choice-based dialogue, probably with the mafia goon
            {
                if (choiceIndex == 0)
                {
                    Dialogue dialogue = new Dialogue(_customerName, _currentCustomer.lines["choice1"]);
                    _dialogueManager.StartDialogue(dialogue, DialogueType.Leave);
                }
                else
                {
                    Dialogue dialogue = new Dialogue(_customerName, _currentCustomer.lines["choice2"]);
                    _dialogueManager.StartDialogue(dialogue, DialogueType.Leave);
                }
            }
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
                _servedCustomers++;
                if (_servedCustomers == _totalDailyCustomers) GameData.allCustomersServed = true;
                
                _currentCustomer = _dailyCustomers[0];
                switch (_currentCustomer.sprite)
                {
                    case CustomerType.Howard:
                        _customerName = "Inspector";
                        break;
                    case CustomerType.ErnestUnion:
                        _customerName = "B.U. member";
                        break;
                    case CustomerType.MafiaGoon:
                        _customerName = "Mafia Goon";
                        break;
                    case CustomerType.MargaretDrunk:
                        _customerName = "Margaret";
                        break;
                    default:
                        _customerName = _currentCustomer.sprite.ToString();
                        break;
                }
                _dailyCustomers.RemoveAt(0);

                // update guest book json
                UpdateGuestBook();
            
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

        private void UpdateGuestBook()
        {
            GuestList guestList;
            if (!PlayerPrefs.HasKey("GuestBook"))
            {
                string jsonString = Resources.Load<TextAsset>("TextAssets/GuestBookData/GuestBook").text;
                guestList = JsonConvert.DeserializeObject<GuestList>(jsonString);
                PlayerPrefs.SetString("GuestBook", jsonString);
            }
            else
            {
                guestList = JsonConvert.DeserializeObject<GuestList>(PlayerPrefs.GetString("GuestBook"));
            }
            
            List<Guest> _guestsData = guestList.guests;

            _guestsData.Find(guest => guest.nickname == _customerName).isUnlocked = true;

            string updatedJson = JsonConvert.SerializeObject(guestList, Formatting.Indented);
            PlayerPrefs.SetString("GuestBook", updatedJson);
        }
        
        private IEnumerator WaitAndGreetDialogue()
        {
            yield return new WaitForSeconds(timeBeforeDialogue);
            DialogueType dialogueType;
            if (_currentCustomer.lines.ContainsKey("choices")) 
            { 
                dialogueType = DialogueType.MultipleChoice; // we have a choice-based dialogue
            }
            else if (_currentCustomer.sprite is CustomerType.Howard or CustomerType.MafiaGoon or CustomerType.ErnestUnion)
            { 
                dialogueType = DialogueType.NoDrink; // these don't order drinks
            } else
            { 
                dialogueType = DialogueType.Greet; // for all other customers
            }
            _dialogueManager.StartDialogue(new Dialogue(_customerName, _currentCustomer.lines["greet"]), dialogueType);
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
            Debug.Log("Farewell called");
            if (_trinketToDisplay >= 0)
            {
                // Shoot the event
                EventSystemManager.OnTrinketDisplayed(_trinketToDisplay);

                // Reset _trinketToDisplay
                _trinketToDisplay = -1;
            }
            else if (_posterToDisplay > 0)
            {
                EventSystemManager.OnPosterDisplayed(_posterToDisplay);

                _posterToDisplay = -1;
            }
            else
            {
                StartCoroutine(WaitBeforeFadeOut());
            }
        }

        private void SetTrinketToDisplay(int trinketID)
        {
            _trinketToDisplay = trinketID;
        }
        
        private void SetPosterToDisplay(int posterID)
        {
            _posterToDisplay = posterID;
        }

        public void FadeOut()
        {
            Debug.Log("fade out called");
            StartCoroutine(WaitBeforeFadeOut());
        }

        private IEnumerator WaitBeforeFadeOut()
        {
            Debug.Log("waitbeforefadeout");
            yield return new WaitForSeconds(timeBeforeFadeout);
            EventSystemManager.OnCustomerLeaveSound();
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
                        // earning += _currentCustomer.tip / 3;
                        earning += 1f; // watered down correct cocktail
                    }
                    else
                    {
                        dialogue = new Dialogue(_customerName, _currentCustomer.lines["correct"]);
                        // earning += _currentCustomer.tip;
                        earning += 5f; // correct cocktail, no water
                    }
                }
                
                else if (cocktail.type == CocktailType.Wrong) // completely wrong cocktail, a mess
                {
                    dialogue = new Dialogue(_customerName, _currentCustomer.lines["wrong"]);
                    earning = 0; // no tip for a completely wrong cocktail
                }
                
                else // correctly executed cocktail, but not the one the customer ordered
                {
                    if (_currentCustomer.lines.ContainsKey(cocktail.type.ToString())) // if we have a custom line for that cocktail
                    {
                        dialogue = new Dialogue(_customerName, _currentCustomer.lines[cocktail.type.ToString()]);
                        // earning += _currentCustomer.tip / 2; // extra tip
                        earning += 5f;
                    }
                    else if (_currentCustomer.lines.ContainsKey("incorrect")) // if we have a generic line for an incorrect but well-done cocktail
                    {
                        dialogue = new Dialogue(_customerName, _currentCustomer.lines["incorrect"]);
                        earning += 1f;
                    }
                    else // fallback to standard "wrong cocktail" line
                    {
                        dialogue = new Dialogue(_customerName, _currentCustomer.lines["wrong"]);
                        earning += 1f;
                    }
                    // earning += _currentCustomer.tip / 3;
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
                
                // Log cocktail
                GameData.Log.Add(new Tuple<string, string>("Bartender", "<i>Serves a " +
                    (cocktail.type == CocktailType.Wrong ? "messy concoction" : // special message if cocktail was wrong
                    $"{Regex.Replace(cocktail.type.ToString(), @"([a-z])([A-Z])", "$1 $2")}") // cocktail name
                    + (cocktail.isWatered ? ", watered down" : "") // whether it was watered down or not
                    + "</i>"));
            }
            else
            {
                // The control flow enters here in case of tutorial
                
                customerCocktail.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/Cocktails/Ripple/Ripple_tot");
                customerCocktail.GetComponent<FadeCanvas>().FadeIn();

                float earning = 5;
                pricePopup.DisplayPrice(earning);
                GameData.TodayEarnings += earning;
            }
            
        }
    }
}
