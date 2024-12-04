using System;
using Technical;
using UnityEngine;
using UnityEngine.UI;

namespace Bar
{
    public class TimerManager : MonoBehaviour
    {
        public float initialTime;
        private float timeRemaining; // 2 minutes
        public bool isOn = false;
        public bool isRunning = false;

        public CanvasGroup timerCanvas;
        [SerializeField] private Image pocketWatchBase;
        [SerializeField] private Image pocketWatchHand;

        void Start() {
            timeRemaining = initialTime;
        }

        void Update()
        {
            if (isRunning)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    float rotationAngle = 360 * timeRemaining/initialTime;
                    pocketWatchHand.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
                }
                else
                {
                    print("Time has run out!");
                    timeRemaining = 0;
                    isRunning = false;
                    EventSystemManager.OnTimeUp();
                }
            }
        }
        
        public void StartTimer()
        {
            timeRemaining = Day.DailyTime;
            isOn = true;
            isRunning = true;
            print($"Timer started. Time remaining: {timeRemaining}");
        }

        public void PauseTimer()
        {
            if (!isOn) return;
            isRunning = false;
            print($"Timer paused. Time remaining: {timeRemaining}");
        }

        public void ResumeTimer()
        {
            if (!isOn) return;
            if (timeRemaining > 0) isRunning = true;
            print($"Timer resumed. Time remaining: {timeRemaining}");
        }
    }
}