using Technical;
using UnityEngine;
using UnityEngine.UI;

public class BlitzTimer : MonoBehaviour
{
    public Image timerBarImage;  
    public float timerDuration;
    private float timeRemaining;
    private bool isTimerRunning;
    
    public void StartTimer()
    {
        timeRemaining = timerDuration; 
        isTimerRunning = true;
        timerBarImage.fillAmount = 1f;  
    }

    void Update()
    {
        if (isTimerRunning)
        {
            timeRemaining -= Time.deltaTime;
            float fillAmount = Mathf.Clamp01(timeRemaining / timerDuration);
            timerBarImage.fillAmount = fillAmount;

            if (timeRemaining <= 0f)
            {
                isTimerRunning = false;
                EventSystemManager.OnBlitzTimerEnded();
            }
        }
    }
}
