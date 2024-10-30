using System.Collections;
using UnityEngine;

public class CanvasFadeAnimation : MonoBehaviour
{
    private bool _isFading = false;
    
    [Range(0.1f, 10.0f)]
    public float fadeSpeed = 1f;
    
    public CanvasGroup canvasGroup;

    public void FadeIn()
    {
        // activates object to start fading
        gameObject.SetActive(true);
        
        StartCoroutine(FadeInCoroutine());
    }
    
    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine());
    }
    
    private IEnumerator FadeInCoroutine()
    {
        if (_isFading) yield break;
        _isFading = true;
        
        canvasGroup.alpha = 0.0f;
        while (canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha +=  fadeSpeed * Time.deltaTime;
            yield return null;
        }

        _isFading = false;
    }

    private IEnumerator FadeOutCoroutine()
    {
        if (_isFading) yield break;
        _isFading = true;

        canvasGroup.alpha = 1.0f;
        while (canvasGroup.alpha > 0.0f)
        {
            canvasGroup.alpha -=  fadeSpeed * Time.deltaTime;
            yield return null;
        }

        _isFading = false;
        
        // deactivates object when done fading inside the coroutine
        gameObject.SetActive(false);
    }
    
}
