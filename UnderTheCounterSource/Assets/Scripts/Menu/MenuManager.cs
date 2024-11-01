using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public CanvasGroup mainMenuCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        mainMenuCanvas.GetComponent<CanvasFadeAnimation>().FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGame()
    {
        SceneManager.LoadScene("Scenes/BarView");
    }
    
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
