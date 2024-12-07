using System;
using Technical;
using UnityEngine;

namespace CocktailCreation
{
    public class WaterIconScript : MonoBehaviour
    {
        void Start()
        {
            EventSystemManager.OnCocktailWatered += ActivateIcon;
            EventSystemManager.OnCCAHided += DeactivateIcon;
        }

        private void OnDestroy()
        {
            EventSystemManager.OnCocktailWatered -= ActivateIcon;
            EventSystemManager.OnCCAHided -= DeactivateIcon;
        }

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
