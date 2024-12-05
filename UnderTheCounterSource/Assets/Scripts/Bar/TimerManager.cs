using System;
using Technical;
using UnityEngine;
using UnityEngine.UI;

namespace Bar
{
    public class TimerManager : MonoBehaviour
    {
        private float initialTime;
        private float timeRemaining;
        public bool isOn = false;
        public bool isRunning = false;
        
        [SerializeField] private Image wallClockHour;
        [SerializeField] private Image wallClockMinute;
        private readonly float timeCoherenceRotation = 100f;

        void Start()
        {
            
        }

        public void SetTime()
        {
            initialTime = Day.DailyTime;
            timeRemaining = initialTime;
        }

        void Update()
        {
            if (isRunning)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    float rotationAngle = 180 * (timeRemaining/initialTime - 1); // 180 means half a clock
                    wallClockHour.transform.rotation = Quaternion.Euler(0, 0, timeCoherenceRotation + rotationAngle);
                    wallClockMinute.transform.rotation = Quaternion.Euler(0, 0, timeCoherenceRotation + rotationAngle*12);
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