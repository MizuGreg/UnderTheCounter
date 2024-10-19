using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
