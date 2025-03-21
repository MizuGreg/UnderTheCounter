using SavedGameData;
using Technical;
using UnityEngine;
using UnityEngine.UI;

public class BlitzTimer : MonoBehaviour
{
    public Image timerBarImage;  
    public float timerDuration;
    private float timeRemaining;
    private bool isTimerRunning;

    private void Start()
    {
        EventSystemManager.OnBlitzCalled += StartTimer;
        EventSystemManager.OnBlitzEnd += ResetTimer;
        EventSystemManager.OnMinigameEnd += StopTimer;

        SetTimer();
    }
    
    private void OnDestroy()
    {
        EventSystemManager.OnBlitzCalled -= StartTimer;
        EventSystemManager.OnBlitzEnd -= ResetTimer;
        EventSystemManager.OnMinigameEnd -= StopTimer;
    }
    
    public void SetTimer()
    {
        timerDuration = GameData.BlitzTime;
        timeRemaining = timerDuration;
        isTimerRunning = false;
        timerBarImage.fillAmount = 1f;
    }
    
    public void StartTimer()
    {
        isTimerRunning = true;  
    }

    public void Update()
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

    private void ResetTimer()
    {
        timeRemaining = timerDuration;
        isTimerRunning = false;
        timerBarImage.fillAmount = 1f;
    }

    private void StopTimer()
    {
        isTimerRunning = false;
    }
}
