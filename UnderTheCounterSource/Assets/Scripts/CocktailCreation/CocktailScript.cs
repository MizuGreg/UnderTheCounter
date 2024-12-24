using Technical;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
            GetComponent<Image>().color = new Color(0.9f, 0.95f, 1, 1f);
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
