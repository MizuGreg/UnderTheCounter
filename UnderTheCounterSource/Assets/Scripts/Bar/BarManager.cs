using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class BarManager : MonoBehaviour
{
    public CanvasGroup barCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        barCanvas.GetComponent<CanvasFadeAnimation>().FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
