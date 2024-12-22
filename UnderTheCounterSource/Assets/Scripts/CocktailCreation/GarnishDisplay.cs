using System;
using Technical;
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
        
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Cocktail"))
            {
                EventSystemManager.OnGarnishAdded();
            }
        }
    }
}
