using System;
using UnityEngine;
using UnityEngine.UI;

namespace CocktailCreation
{
    public class GarnishDisplay : Draggable
    {
        public Garnish garnish;

        private void Start()
        {
            this.GetComponent<Image>().sprite = garnish.sprite;
        }
    }
}
