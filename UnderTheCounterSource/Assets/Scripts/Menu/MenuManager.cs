using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        public CanvasGroup mainMenuCanvas;
    
        void Start()
        {
            mainMenuCanvas.GetComponent<CanvasFadeAnimation>().FadeIn();
            EventSystemManager.OnLoadMainMenu();
        }

        public void StartNewGame()
        {
            SceneManager.LoadScene("Scenes/BarView");
        }
    
    
        public void QuitGame()
        {
            Technical.QuitGame.Quit();
        }
    }
}
