using System.Collections;
using Technical;
using UnityEngine;

namespace Blitz
{
    public class BlitzManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup blitzCanvas;
        [SerializeField] private BlitzTimer blitzTimer;

        private void Start()
        {
            EventSystemManager.OnBlitzCalled += CallBlitz;
            EventSystemManager.OnBlitzTimerEnded += EndHideMinigame;
        }

        public void CallBlitz()
        {
            blitzCanvas.GetComponent<FadeCanvas>().FadeIn();
            StartCoroutine(WaitBeforeHideMinigame());
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
