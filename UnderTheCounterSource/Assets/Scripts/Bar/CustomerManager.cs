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
        private List<Customer> dailyCustomers;
        private Customer currentCustomer;
        private Image currentImage;
        private DialogueManager dialogueManager;

        public CanvasGroup customerCanvas;
        
        public float timeBetweenCustomers = 2.5f;

        void Start()
        {
            loadDailyCustomers(DaySO.currentDay);
            EventSystemManager.OnTimeUp += timeoutCustomers;
            EventSystemManager.OnPreparationStart += prepareCCA;
            EventSystemManager.OnCocktailMade += serveCustomer;
            EventSystemManager.OnCustomerLeave += farewellCustomer;
        
            currentImage = customerCanvas.transform.Find("CustomerSprite").gameObject.GetComponent<Image>();
        }

        public void attachDialogueManager(DialogueManager dialogueManager)
        {
            this.dialogueManager = dialogueManager;
        }

        private void timeoutCustomers()
        {
            dailyCustomers.Clear(); // depletes daily customers
        }

        private void loadDailyCustomers(int currentDay)
        {
            // read DailyCustomers json and create daily customers list
            string jsonString = File.ReadAllText("Assets/Data/CustomerData/Day" + currentDay + ".json");
            // dailyCustomers = JsonUtility.FromJson<CustomerList>(jsonString).customers;
            
            dailyCustomers = JsonConvert.DeserializeObject<CustomerList>(jsonString).customers;
            
            print(dailyCustomers[0]);
        }

        public void greetCustomer()
        {
            // pick the first customer from the list and set it to current. if empty we throw onCustomersDepleted
            // otherwise send out lines to display to the dialogue manager
            if (dailyCustomers.Count > 0)
            {
                currentCustomer = dailyCustomers[0];
                dailyCustomers.RemoveAt(0);
            
                currentImage.sprite = getSpriteFromCustomerType(currentCustomer.sprite);
                customerCanvas.GetComponent<CanvasFadeAnimation>().FadeIn();
                StartCoroutine(waitAndGreet());
                EventSystemManager.OnCustomerEnter();
            }
            else
            {
                EventSystemManager.OnCustomersDepleted();
            }
        }

        private Sprite getSpriteFromCustomerType(CustomerType customerType)
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

        private IEnumerator waitAndGreet()
        {
            yield return new WaitForSeconds(3);
            print(currentCustomer);
            dialogueManager.customerGreet(currentCustomer.lines["greet"]);
        }

        public void farewellCustomer()
        {
            // fade out customer, wait a bit, then call greetCustomer for next customer
            customerCanvas.GetComponent<CanvasFadeAnimation>().FadeOut();
            EventSystemManager.OnCustomerLeave();
            StartCoroutine(waitBeforeNextCustomer());
        }

        private IEnumerator waitBeforeNextCustomer()
        {
            yield return new WaitForSeconds(timeBetweenCustomers);
            greetCustomer();
        }

        public void prepareCCA()
        {
            // we throw event onMakeCocktail with current customer's cocktail order, with wateredDown false by default
            EventSystemManager.OnMakeCocktail(new Cocktail(currentCustomer.order, wateredDown: false));
        }

        public void serveCustomer(Cocktail cocktail)
        {
            // we compare with current customer's cocktail, call dialogue line in dialogue manager accordingly
            CocktailType order = currentCustomer.order;
            if (cocktail.type != order) dialogueManager.customerServe(currentCustomer.lines["leaveWrong"]);
            else if (cocktail.wateredDown) dialogueManager.customerServe(currentCustomer.lines["leaveWater"]);
            else dialogueManager.customerServe(currentCustomer.lines["leaveCorrect"]);
            
            // todo: add money system
            
            // if not watered down, we throw onDrunkCustomer event
            if (!cocktail.wateredDown) EventSystemManager.OnDrunkCustomerLeave();
        }
    

        public void playTutorial()
        {
            // todo: tutorial with ex bar owner. for now it's just a simple customer, but it could become something more
            // complex in the future if need be
            greetCustomer();
        }
    }
}
