using System.Collections;
using Bar;
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
            mainMenuCanvas.GetComponent<FadeCanvas>().FadeOut();
            Day.Initialize();
            StartCoroutine(WaitBeforeNewGame());
        }

        private IEnumerator WaitBeforeNewGame()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Scenes/IntroductionScreen");
        }
    
        public void QuitGame()
        {
            Technical.QuitGame.Quit();
        }
    }
}
