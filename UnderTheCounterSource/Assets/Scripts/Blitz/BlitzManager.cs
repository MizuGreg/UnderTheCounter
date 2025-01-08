using System.Collections;
using Technical;
using TMPro;
using UnityEngine;
using Bar;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using SavedGameData;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.Serialization;

namespace Blitz
{
    public class BlitzManager : MonoBehaviour
    {
        private List<Customer> _dailyBlitzAppearances;
        private Customer _currentAppearance;
        private DialogueManager _dialogueManager;
        
        [SerializeField] private CanvasGroup blitzCanvas;
        [SerializeField] private BlitzTimer blitzTimer;
        [SerializeField] private FadeCanvas warningPopup;

        [SerializeField] private CanvasGroup barContainer;
        [SerializeField] private List<GameObject> placeholderSlots;
        private int placedBottlesCounter;

        private void Start()
        {
            warningPopup.gameObject.SetActive(false);
            
            EventSystemManager.OnBlitzCallWarning += BlitzWarning;
            EventSystemManager.OnBlitzCalled += CallBlitz;
            EventSystemManager.OnBlitzTimerEnded += LossByBlitz;
            EventSystemManager.OnBottlePlaced += IncreasePlacedBottlesCounter;
        }

        private void OnDestroy()
        {
            EventSystemManager.OnBlitzCallWarning -= BlitzWarning;
            EventSystemManager.OnBlitzCalled -= CallBlitz;
            EventSystemManager.OnBlitzTimerEnded -= LossByBlitz;
            EventSystemManager.OnBottlePlaced -= IncreasePlacedBottlesCounter;
        }

        private void BlitzWarning()
        {
            StartCoroutine(BlinkWarning());
        }

        private IEnumerator BlinkWarning()
        {
            warningPopup.FadeIn();
            yield return new WaitForSeconds(3f);
            warningPopup.FadeOut();
        }
        
        public void CallBlitz()
        {
            StartCoroutine(FadeInBlitz());
        }

        private IEnumerator FadeInBlitz()
        {
            yield return new WaitForSeconds(1f);
            placedBottlesCounter = 0;
            ShufflePlaceholders();
            blitzCanvas.GetComponent<FadeCanvas>().FadeIn();
            StartCoroutine(WaitBeforeHideMinigame());
        }

        private void ShufflePlaceholders()
        {
            GameObject[] ingredientPrefabs = Resources.LoadAll<GameObject>("Prefabs/Blitz");
            List<GameObject> shuffledPrefabs = new List<GameObject>(ingredientPrefabs);
            Shuffle(shuffledPrefabs);

            for (int i = 0; i < shuffledPrefabs.Count; i++)
            {
                placeholderSlots[i].tag = shuffledPrefabs[i].GetComponent<PlaceholderScript>().ingredientType.ToString();
                placeholderSlots[i].GetComponent<Image>().sprite = shuffledPrefabs[i].GetComponent<PlaceholderScript>().sprite;
            }
        }

        private void Shuffle(List<GameObject> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int rnd = Random.Range(i, list.Count);
                var temp = list[i];
                list[i] = list[rnd];
                list[rnd] = temp;
            }
        }

        private IEnumerator WaitBeforeHideMinigame()
        {
            yield return new WaitForSeconds(1f);
            blitzTimer.StartTimer();
        }

        private void IncreasePlacedBottlesCounter()
        {
            placedBottlesCounter++;
            CheckBlitzWin();
        }

        private void CheckBlitzWin()
        {
            if (placedBottlesCounter == AssetDatabase.FindAssets("", new[] { $"Assets/Resources/Prefabs/Blitz" }).Length) 
            {
                // we need some kind of confirmation to show up for the player, then wait a bit, and then fade out
                blitzCanvas.GetComponent<FadeCanvas>().FadeOut();
                EventSystemManager.OnMinigameEnd();
            }
        }
        
        private void LossByBlitz()
        {
            barContainer.GetComponent<FadeCanvas>().FadeOut();
            StartCoroutine(LoadLoseScreen());
        }
         
        private IEnumerator LoadLoseScreen()
        {
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("GameOverScreen");
        }
    }
}
