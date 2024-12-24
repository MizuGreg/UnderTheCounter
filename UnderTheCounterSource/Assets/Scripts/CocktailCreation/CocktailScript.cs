using Technical;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CocktailCreation
{
    public class CocktailScript : MonoBehaviour, IDropHandler
    {
        public Cocktail cocktail;
        [SerializeField] private Sprite sprite;
        [SerializeField] private GameObject garnish;

        public void WaterDownCocktail()
        {
            cocktail.isWatered = true;
            Debug.Log("cocktail watered");
        }

        public void OnDrop(PointerEventData eventData)
        {
            // Check if the dropped object is a garnish
            if(eventData.pointerDrag!.CompareTag("Garnish"))
            {
                EventSystemManager.OnGarnishAdded();
            }
        }

        public GameObject GetGarnish()
        {
            return this.garnish;
        }

    }
}
