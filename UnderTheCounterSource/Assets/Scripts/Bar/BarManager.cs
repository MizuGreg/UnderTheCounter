using UnityEngine;
using System.Collections;
using Bar;
using Unity.VisualScripting;

public class BarManager : MonoBehaviour
{
    private CustomerManager customerManager;
    private CCAManager CCAManager;
    private RecipeBookManager recipeBookManager;
    private TimerManager timerManager;
    
    public CanvasGroup barCanvas;
    // todo: pocket watch canvas
    
    // Start is called before the first frame update
    void Start()
    {
        customerManager = GetComponent<CustomerManager>();
        CCAManager = GetComponent<CCAManager>();
        recipeBookManager = GetComponent<RecipeBookManager>();
        timerManager = GetComponent<TimerManager>();
        
        barCanvas.GetComponent<CanvasFadeAnimation>().FadeIn();
        EventSystemManager.OnDayStart += startDay;
        EventSystemManager.OnDrunkCustomerLeave += checkDrunk;
    }

    public void startDay()
    {
        if (DaySO.currentDay == 1) customerManager.playTutorial();
        else customerManager.greetCustomer();
        timerManager.startTimer();
        
    }

    private void checkDrunk()
    {
        
        if (DaySO.drunkCustomers++ >= DaySO.maxDrunkCustomers) callBlitz();
    }

    private void callBlitz()
    {
        // todo blitz. should probably be a coroutine
        // for now:
        lossByBlitz();
    }

    private void lossByBlitz()
    {
        // todo: display loss screen
    }
}
