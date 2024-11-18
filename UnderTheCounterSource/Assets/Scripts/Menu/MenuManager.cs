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
            SceneManager.LoadScene("Scenes/BarView");
        }
    
        public void QuitGame()
        {
            Technical.QuitGame.Quit();
        }
    }
}
