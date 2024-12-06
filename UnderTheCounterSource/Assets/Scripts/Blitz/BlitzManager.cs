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
        
        [SerializeField] private TextMeshProUGUI blitzPlaceholderText;
        
        private bool blitzActive = false;

        private void Start()
        {
            blitzPlaceholderText.gameObject.SetActive(false);
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
            blitzPlaceholderText.GetComponent<FadeCanvas>().FadeIn();
            yield return new WaitForSeconds(3f);
            blitzPlaceholderText.GetComponent<FadeCanvas>().FadeOut();
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
