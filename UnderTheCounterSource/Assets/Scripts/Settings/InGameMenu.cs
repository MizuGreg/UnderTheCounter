using Bar;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    private TimerManager _timerManager;
    
    void Awake()
    {
        _timerManager = GameObject.FindWithTag("BarManager").transform.GetComponent<TimerManager>();
    }

    private void OnEnable()
    {
        _timerManager.pauseTimer();
    }

    private void OnDisable()
    {
        _timerManager.resumeTimer();
    }
}
