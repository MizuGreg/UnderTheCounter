using UnityEngine;
using UnityEngine.Events;

public static class EventSystemManager
{
    public static UnityAction onDayStart; // event is called either from shop window view or from bar manager's start()
    public static UnityAction onDayEnd;
    public static UnityAction onTimeUp;
    
    public static UnityAction OnCustomerEnter;
    public static UnityAction OnCustomerLeave;
    public static UnityAction OnDrunkCustomerLeave;
    public static UnityAction onCustomersDepleted;
    
    public static UnityAction onPreparationStart;
    public static UnityAction<CCAManager.Cocktail> onMakeCocktail;
    public static UnityAction<CCAManager.Cocktail> onCocktailMade;
}