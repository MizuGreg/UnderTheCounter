using Technical;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CocktailCreation
{
    public class CocktailScript : MonoBehaviour, IDropHandler
    {
        public Cocktail cocktail;
        [SerializeField] private Sprite sprite;

        public void WaterDownCocktail()
        {
            cocktail.isWatered = true;
            print("Cocktail watered");
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            // Check if the dropped object is a garnish
            if(eventData.pointerDrag!.CompareTag("Garnish"))
            {
                EventSystemManager.OnGarnishAdded();
            }
        }

    }
}
