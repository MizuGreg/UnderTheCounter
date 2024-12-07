using System;
using Technical;
using UnityEngine;

namespace CocktailCreation
{
    public class WaterIconScript : MonoBehaviour
    {

        private void ActivateIcon()
        {
            gameObject.SetActive(true);
        }

        private void DeactivateIcon()
        {
            gameObject.SetActive(false);
        }
    }
}
