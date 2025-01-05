using System.Collections.Generic;
using UnityEngine;

namespace CocktailCreation
{
    public class FullnessBar : MonoBehaviour
    {
        [SerializeField] private GameObject slot1;
        [SerializeField] private GameObject slot2;
        [SerializeField] private GameObject slot3;
        [SerializeField] private GameObject slot4;
        [SerializeField] private GameObject slot5;

        [SerializeField] private Color verlanColor;
        [SerializeField] private Color caledonColor;
        [SerializeField] private Color ferrucciColor;
        [SerializeField] private Color gryteColor;
        [SerializeField] private Color shaddockColor;
        [SerializeField] private Color canticoColor;
        

        private List<GameObject> _slotList = new();
        
        void Start()
        {
            CreateSlotList();
        }

        public void ColorBarSlot(int index, IngredientType ingredient)
        {
            _slotList[index-1].GetComponent<BarSlot>().SetColor(RetrieveColor(ingredient));
        }

        private Color RetrieveColor(IngredientType ingredient)
        {
            switch (ingredient)
            {
                case IngredientType.Verlan:
                    return verlanColor;
                case IngredientType.Caledon:
                    return caledonColor;
                case IngredientType.Ferrucci:
                    return ferrucciColor;
                case IngredientType.Gryte:
                    return gryteColor;
                case IngredientType.Shaddock:
                    return shaddockColor;
                default:
                    return canticoColor;
            }
        }

        private void CreateSlotList()
        {
            _slotList.Add(slot1);
            _slotList.Add(slot2);
            _slotList.Add(slot3);
            _slotList.Add(slot4);
            _slotList.Add(slot5);
        }

        public void ResetBar()
        {
            foreach (var slot in _slotList)
            {
                slot.GetComponent<BarSlot>().Hide();
            }
        }
        
    }
}
