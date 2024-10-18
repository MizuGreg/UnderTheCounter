using System.Collections;
using UnityEngine;

public class CanvasFadeAnimation : MonoBehaviour
{
    private bool _fadeIn, _fadeOut;
    
    [Range(0.0f, 10.0f)]
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
        if (IsFading()) yield break;
        _fadeIn = true;
        
        canvasGroup.alpha = 0.0f;
        while (canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha +=  fadeSpeed * Time.deltaTime;
            yield return null;
        }

        _fadeIn = false;
    }

    private IEnumerator FadeOutCoroutine()
    {
        if (IsFading()) yield break;
        _fadeOut = true;

        canvasGroup.alpha = 1.0f;
        while (canvasGroup.alpha > 0.0f)
        {
            canvasGroup.alpha -=  fadeSpeed * Time.deltaTime;
            yield return null;
        }

        _fadeOut = false;
        
        // deactivates object when done fading inside the coroutine
        gameObject.SetActive(false);
    }

    private bool IsFading()
    {
        return _fadeIn || _fadeOut;
    }
}
