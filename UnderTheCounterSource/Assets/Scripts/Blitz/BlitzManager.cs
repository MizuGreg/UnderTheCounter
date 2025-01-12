using System.Collections;
using Technical;
using UnityEngine;
using Bar;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using SavedGameData;

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
        [SerializeField] private FadeCanvas blitzIncomingPopup;

        [SerializeField] private CanvasGroup barContainer;
        [SerializeField] private List<GameObject> placeholderSlots;
        [SerializeField] private List<GameObject> placeholderPrefabs;
        private int placedBottlesCounter;

        private void Start()
        {
            warningPopup.gameObject.SetActive(false);
            blitzIncomingPopup.gameObject.SetActive(false);
            
            EventSystemManager.OnBlitzCallWarning += BlitzWarning;
            EventSystemManager.OnBlitzCalled += CallBlitz;
            EventSystemManager.OnBlitzTimerEnded += LossByBlitz;
            EventSystemManager.OnBottlePlaced += IncreasePlacedBottlesCounter;
            
            placeholderPrefabs = new List<GameObject>(Resources.LoadAll<GameObject>("Prefabs/Blitz"));
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
            yield return new WaitForSeconds(2f);
            blitzIncomingPopup.FadeIn();
            // increase timer before fade in for music coherence
            yield return new WaitForSeconds(2.5f);
            placedBottlesCounter = 0;
            ShufflePlaceholders();
            blitzCanvas.GetComponent<FadeCanvas>().FadeIn();
            StartCoroutine(WaitBeforeHideMinigame());
        }

        private void ShufflePlaceholders()
        {
            Shuffle(placeholderPrefabs);

            for (int i = 0; i < placeholderPrefabs.Count; i++)
            {
                placeholderSlots[i].tag = placeholderPrefabs[i].GetComponent<PlaceholderScript>().ingredientType.ToString();
                placeholderSlots[i].GetComponent<Image>().sprite = placeholderPrefabs[i].GetComponent<PlaceholderScript>().sprite;
            }
        }

        private void Shuffle(List<GameObject> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int rnd = Random.Range(i, list.Count);
                (list[i], list[rnd]) = (list[rnd], list[i]); // swaps the two objects
            }
        }

        private IEnumerator WaitBeforeHideMinigame()
        {
            blitzIncomingPopup.FadeOut();
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
            if (placedBottlesCounter == placeholderPrefabs.Count) 
            {
                // we need some kind of confirmation to show up for the player, then wait a bit, and then fade out
                blitzCanvas.GetComponent<FadeCanvas>().FadeOut();
                EventSystemManager.OnMinigameEnd();
            }
        }
        
        private void LossByBlitz()
        {
            StartCoroutine(LoadLoseScreen());
        }
         
        private IEnumerator LoadLoseScreen()
        {
            yield return new WaitForSeconds(2f);
            barContainer.GetComponent<FadeCanvas>().FadeOut();
            EventSystemManager.OnLoadLoseScreen("blitz");
            yield return new WaitForSeconds(2f);
            GameData.loseType = "blitz";
            SceneManager.LoadScene("EndingScene");
        }
    }
}
