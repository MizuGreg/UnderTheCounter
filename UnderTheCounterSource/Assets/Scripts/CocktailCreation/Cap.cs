using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CocktailCreation
{
    public class Cap : Draggable
    {
        [SerializeField] private RectTransform dropSlot;

        private GameObject _gameController;

        private void Start()
        {
            _gameController = GameObject.FindGameObjectWithTag("GameController");
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            CanvasGroup.blocksRaycasts = true;
            
            if (RectTransformUtility.RectangleContainsScreenPoint(dropSlot, Input.mousePosition, canvas.worldCamera))
            {
                _gameController.GetComponent<CocktailManager>().MakeCocktail(dropSlot.GetComponent<Shaker>().GetIngredients());
                dropSlot.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
            else
            {
                ReturnToInitialPosition();
            }
        }
        
    }
}
