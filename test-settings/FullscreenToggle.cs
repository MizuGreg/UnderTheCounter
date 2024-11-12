using UnityEngine;

public class FullscreenToggle : MonoBehaviour
{
    private bool isFullscreen;

    void Start()
    {
        isFullscreen = Screen.fullScreen;
    }

    public void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen;
        Screen.fullScreen = isFullscreen;

        // usato per testare se il gioco è in modalità fullscreen o windowed
        if (isFullscreen)
        {
            Debug.Log("Fullscreen mode is active");
        }
        else
        {
            Debug.Log("Windowed mode is active");
        }
    }
}
