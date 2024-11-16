using System;
using UnityEngine;
using UnityEngine.UI;

namespace Bar.CocktailCreation
{
    public class BarSlot : MonoBehaviour
    {
        [SerializeField] private Color slotColor;

        private Image _image;

        private void Start()
        {
            _image = gameObject.GetComponent<Image>();
            Hide();
        }

        public void SetColor(Color color)
        {
            slotColor = color;
            _image.color = color;
        }

        public void Hide()
        {
            slotColor.a = 0;
            SetColor(slotColor);
        }
        
    }
}
