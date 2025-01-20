using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Technical;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Endings
{
    public class CreditsManager : MonoBehaviour
    {
        public FadeCanvas creditsCanvas;
        public Button backToMenuButton;

        void Start()
        {
            creditsCanvas.FadeIn();
            backToMenuButton.gameObject.SetActive(false);

            StartCoroutine(ShowButtons());
        }

        public IEnumerator ShowButtons()
        {
            yield return new WaitForSeconds(2f);
            backToMenuButton.GetComponent<FadeCanvas>().FadeIn();
        }

        public void LoadMainMenu()
        {
            StartCoroutine(LoadMainMenuScene());
        }

        public IEnumerator LoadMainMenuScene()
        {
            creditsCanvas.FadeOut();
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("MainMenu");
        }
    }
}