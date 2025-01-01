using System.Collections;
using Bar;
using SavedGameData;
using Technical;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        public CanvasGroup mainMenuCanvas;
    
        void Start()
        {
            mainMenuCanvas.GetComponent<FadeCanvas>().FadeIn();
            EventSystemManager.OnLoadMainMenu();
        }

        public void StartNewGame()
        {
            GameData.Initialize();
            mainMenuCanvas.GetComponent<FadeCanvas>().FadeOut();
            StartCoroutine(WaitBeforeNewGame());
        }

        private IEnumerator WaitBeforeNewGame()
        {
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene("IntroductionScreen");
        }

        public void ContinueGame()
        {
            GameData.LoadFromJson();
            StartCoroutine(WaitBeforeContinueGame());
        }

        private IEnumerator WaitBeforeContinueGame()
        {
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene("ShopWindow");
        }
    
        public void QuitGame()
        {
            Technical.QuitGame.Quit();
        }
    }
}
