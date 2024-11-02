using UnityEngine;

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

        protected override void EndDragBehaviour()
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(dropSlot, Input.mousePosition, Canvas.worldCamera))
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
