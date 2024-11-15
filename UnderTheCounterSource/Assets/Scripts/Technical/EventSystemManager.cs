using Bar;
using UnityEngine.Events;

public static class EventSystemManager
{
    public static UnityAction OnDayStart; // event is called either from shop window view or from bar manager's start()
    public static UnityAction OnDayEnd;
    public static UnityAction OnTimeUp;
    
    public static UnityAction OnCustomerEnter;
    public static UnityAction OnCustomerLeave;
    public static UnityAction OnDrunkCustomerLeave;
    public static UnityAction OnCustomersDepleted;
    
    public static UnityAction OnPreparationStart;
    public static UnityAction OnMakeCocktail;
    public static UnityAction<Cocktail> OnCocktailMade;

    public static UnityAction OnLoadMainMenu;
    public static UnityAction OnLoadBarView;
}