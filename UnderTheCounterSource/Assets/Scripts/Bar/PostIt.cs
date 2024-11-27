using System;
using CocktailCreation;
using Technical;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bar
{
    public class PostIt : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI cocktailName;

        private void Awake() {
        }

        private void OnDestroy() {
        }

        public void WriteCocktail(CocktailType cocktailType) {
            if (cocktailType != CocktailType.Wrong) {
                cocktailName.text = cocktailType.ToString();
                // edge case here... should be handled better and not hardcoded
                if (cocktailName.text == "SpringBee") cocktailName.text = "Spring Bee";
                GetComponent<FadeCanvas>().FadeIn();
            }
        }

        public void HidePostIt() {
            if (gameObject.activeSelf) GetComponent<FadeCanvas>().FadeOut();
        }
        
    }
}