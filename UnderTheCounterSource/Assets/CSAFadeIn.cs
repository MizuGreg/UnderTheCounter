using System.Collections;
using System.Collections.Generic;
using Technical;
using UnityEngine;

public class CSAFadeIn : MonoBehaviour
{
    [SerializeField] private FadeCanvas CSA;
    [SerializeField] private FadeCanvas overlay;
    [SerializeField] private FadeCanvas cocktail;
    [SerializeField] private FadeCanvas buttons;

    [SerializeField] private float waitingTime;

    private void Start()
    {
        
    }

    private void SetContentInactive()
    {
        overlay.gameObject.SetActive(false);
        cocktail.gameObject.SetActive(false);
        buttons.gameObject.SetActive(false);
    }
    
    public void OnEnable()
    {
        SetContentInactive();
        StartCoroutine(FadeInGradually());
    }

    private IEnumerator FadeInGradually()
    {
        yield return new WaitForSeconds(waitingTime);
        overlay.gameObject.SetActive(true);
        overlay.FadeIn();
        yield return new WaitForSeconds(waitingTime);
        cocktail.gameObject.SetActive(true);
        cocktail.FadeIn();
        yield return new WaitForSeconds(waitingTime);
        buttons.gameObject.SetActive(true);
        buttons.FadeIn();
    }

    private void OnDisable()
    {
        
    }

}
