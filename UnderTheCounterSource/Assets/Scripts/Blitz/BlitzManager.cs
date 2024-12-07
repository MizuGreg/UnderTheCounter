using System.Collections;
using Technical;
using TMPro;
using UnityEngine;

namespace Blitz
{
    public class BlitzManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup blitzCanvas;
        [SerializeField] private BlitzTimer blitzTimer;
        
        [SerializeField] private FadeCanvas blitzPlaceholder;
        
        private bool blitzActive = false;

        private void Start()
        {
            blitzPlaceholder.gameObject.SetActive(false);
            EventSystemManager.OnBlitzCalled += CallBlitz;
            EventSystemManager.OnBlitzTimerEnded += EndHideMinigame;
        }

        private void OnDestroy()
        {
            EventSystemManager.OnBlitzCalled -= CallBlitz;
            EventSystemManager.OnBlitzTimerEnded -= EndHideMinigame;
        }

        public void CallBlitz()
        {
            if (blitzActive)
            {
                blitzCanvas.GetComponent<FadeCanvas>().FadeIn();
                StartCoroutine(WaitBeforeHideMinigame());
            }
            else
            {
                StartCoroutine(BlinkPlaceholderText());
            }
        }

        private IEnumerator BlinkPlaceholderText()
        {
            blitzPlaceholder.FadeIn();
            yield return new WaitForSeconds(3f);
            blitzPlaceholder.FadeOut();
        }

        private IEnumerator WaitBeforeHideMinigame()
        {
            yield return new WaitForSeconds(1f);
            blitzTimer.StartTimer();
        }

        private void EndHideMinigame()
        {
            StartCoroutine(WaitBeforeEndHideMinigame());
        }

        private IEnumerator WaitBeforeEndHideMinigame()
        {
            yield return new WaitForSeconds(1f);
            blitzCanvas.GetComponent<FadeCanvas>().FadeOut();
        }
    }
}
