using System.Collections;
using Technical;
using TMPro;
using UnityEngine;
using Bar;
using UnityEngine.SceneManagement;

namespace Blitz
{
    public class BlitzManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup blitzCanvas;
        [SerializeField] private BlitzTimer blitzTimer;
        
        [SerializeField] private FadeCanvas warningPlaceholder;

        [SerializeField] private CanvasGroup barContainer;
        private int placedBottlesCounter;

        private void Start()
        {
            warningPlaceholder.gameObject.SetActive(false);
            
            EventSystemManager.OnBlitzCallWarning += BlitzWarning;
            EventSystemManager.OnBlitzCalled += CallBlitz;
            EventSystemManager.OnBlitzTimerEnded += LossByBlitz;
            EventSystemManager.OnBottlePlaced += IncreasePlacedBottlesCounter;
            EventSystemManager.OnPanelClosed += CheckBlitzWin;
        }

        private void OnDestroy()
        {
            EventSystemManager.OnBlitzCallWarning -= BlitzWarning;
            EventSystemManager.OnBlitzCalled -= CallBlitz;
            EventSystemManager.OnBlitzTimerEnded -= LossByBlitz;
            EventSystemManager.OnBottlePlaced -= IncreasePlacedBottlesCounter;
            EventSystemManager.OnPanelClosed -= CheckBlitzWin;
        }

        private void BlitzWarning()
        {
            StartCoroutine(BlinkPlaceholderText());
        }

        private IEnumerator BlinkPlaceholderText()
        {
            warningPlaceholder.FadeIn();
            yield return new WaitForSeconds(3f);
            warningPlaceholder.FadeOut();
        }
        
        public void CallBlitz()
        {
            StartCoroutine(FadeInBlitz());
        }

        private IEnumerator FadeInBlitz()
        {
            yield return new WaitForSeconds(1f);
            placedBottlesCounter = 0;
            blitzCanvas.GetComponent<FadeCanvas>().FadeIn();
            StartCoroutine(WaitBeforeHideMinigame());
        }

        private IEnumerator WaitBeforeHideMinigame()
        {
            yield return new WaitForSeconds(1f);
            blitzTimer.StartTimer();
        }

        private void LossByBlitz()
        {
            barContainer.GetComponent<FadeCanvas>().FadeOut();
            StartCoroutine(LoadLoseScreen());
        }

        private IEnumerator LoadLoseScreen()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("GameOverScreen");
        }

        private void IncreasePlacedBottlesCounter()
        {
            placedBottlesCounter++;
        }

        private void CheckBlitzWin()
        {
            if (placedBottlesCounter == 2) // terrible hardcoding, also the ingredients should be 3 not 2
            {
                blitzCanvas.GetComponent<FadeCanvas>().FadeOut();
                // panel needs to be not movable anymore
                // we need some kind of confirmation to show up for the player, then wait a bit, and then fade out
            }
        }
    }
}
