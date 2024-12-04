using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Technical;

namespace Endings
{
    public class GameOverManager : MonoBehaviour
    {
        public FadeCanvas gameOverCanvas;
        public Button backToMenuButton;

        void Start()
        {
            gameOverCanvas.FadeIn();
            backToMenuButton.gameObject.SetActive(false);

            StartCoroutine(ShowButtons());
        }

        private IEnumerator ShowButtons()
        {
            yield return new WaitForSeconds(2f);
            backToMenuButton.gameObject.SetActive(true);
        }
    }
}