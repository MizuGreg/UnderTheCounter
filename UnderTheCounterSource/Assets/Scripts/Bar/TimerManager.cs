using System;
using Technical;
using UnityEngine;

namespace Bar
{
    public class TimerManager : MonoBehaviour
    {
        public float timeRemaining; // 2 minutes
        public bool isRunning = false;

        public CanvasGroup timerCanvas;
        
        public void startTimer()
        {
            timeRemaining = Day.DailyTime;
            isRunning = true;
            print($"Timer started. Time remaining: {timeRemaining}");
        }

        public void pauseTimer()
        {
            isRunning = false;
            print($"Timer paused. Time remaining: {timeRemaining}");
        }

        public void resumeTimer()
        {
            if (timeRemaining > 0) isRunning = true;
            print($"Timer resumed. Time remaining: {timeRemaining}");
        }

        void Update()
        {
            if (isRunning)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                }
                else
                {
                    print("Time has run out!");
                    timeRemaining = 0;
                    isRunning = false;
                    EventSystemManager.OnTimeUp();
                    // todo: stop clock arm as well
                }
            }
        }
    }
}