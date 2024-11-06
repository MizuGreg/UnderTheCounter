using UnityEngine;
using System.Collections;
using Bar;

public class BarManager : MonoBehaviour
{
    private CustomerManager customerManager;
    private CCAManager CCAManager;
    private RecipeBookManager recipeBookManager;
    
    public CanvasGroup barCanvas;
    // todo: pocket watch canvas
    
    // Start is called before the first frame update
    void Start()
    {
        barCanvas.GetComponent<CanvasFadeAnimation>().FadeIn();
        EventSystemManager.onDayStart += OnDayStart;
    }

    private void OnDayStart()
    {
        if (DaySO.currentDay == 1) customerManager.playTutorial();
        else customerManager.greetCustomer();
    }
}
