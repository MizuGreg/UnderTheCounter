using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Technical;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Endings
{
    public class GameOverManager : MonoBehaviour
    {
        public FadeCanvas gameOverCanvas;
        public Button backToMenuButton;

        void Start()
        {
            EventSystemManager.OnLoadLoseScreen();
            
            gameOverCanvas.FadeIn();
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
            gameOverCanvas.FadeOut();
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("MainMenu");
        }
    }
}