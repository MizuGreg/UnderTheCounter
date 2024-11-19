using System.Collections;
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
            StartCoroutine(WaitBeforeNewGame());
        }

        private IEnumerator WaitBeforeNewGame()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Scenes/BarView");
        }
    
        public void QuitGame()
        {
            Technical.QuitGame.Quit();
        }
    }
}
