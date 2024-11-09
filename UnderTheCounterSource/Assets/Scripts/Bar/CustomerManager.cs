using System;
using System.Collections.Generic;
using Bar;
using UnityEngine;
using UnityEngine.Events;

public class CustomerManager : MonoBehaviour
{
    private List<Customer> dailyCustomers;
    private Customer currentCustomer;
    private DialogueManager dialogueManager;

    public CanvasGroup customerCanvas;
    
    public enum CustomerType
    {
        Margaret,
        Helene,
        Charles,
        Betty,
        Eugene,
        Doris,
        Kenneth,
        Kathryn,
        Willie,
        Louis,
        Ernest, // ex-bar owner
        Howard, // inspector
        Unspecified
        
    }

    private struct Customer
    {
        private CustomerType sprite;
        private CCAManager.CocktailType cocktail;
        private Dictionary<string, string> lines;
    }

    void Start()
    {
        loadDailyCustomers(DaySO.currentDay);
        EventSystemManager.OnTimeUp += timeoutCustomers;
        EventSystemManager.OnPreparationStart += prepareCCA;
        EventSystemManager.OnCocktailMade += serveCustomer;
        EventSystemManager.OnCustomerLeave += farewellCustomer;
    }

    private void timeoutCustomers()
    {
        // todo: deplete customers list
    }

    private void loadDailyCustomers(int currentDay)
    {
        // todo: read DailyCustomers json and create daily customers list
    }

    public void greetCustomer()
    {
        // todo: pick the first customer from the list and set it to current. if empty we throw onCustomersDepleted
        // otherwise send out lines to display to the dialogue manager
    }
// remember to throw events for future sound manager for entering and leaving...
    public void farewellCustomer()
    {
        // todo: fade out customer, wait a bit, then call greetCustomer for next customer
    }

    public void prepareCCA()
    {
        // todo: throw event onMakeCocktail with current customer's cocktail with wateredDown usually false
    }

    public void serveCustomer(CCAManager.Cocktail cocktail)
    {
        // todo: compare with current customer's cocktail, call dialogue line in dialogue manager accordingly
        // if not watered down, we throw onDrunkCustomer event
    }
    

    public void playTutorial()
    {
        // todo: tutorial with ex bar owner. for now it's just a simple customer, but it can become something more custom
        greetCustomer();
    }
}
