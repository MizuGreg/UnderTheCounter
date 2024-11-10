using UnityEngine;

namespace Bar
{
    public class TimerManager : MonoBehaviour
    {
        public float timeRemaining = 120; // 2 minutes
        public bool isRunning = false;

        public CanvasGroup timerCanvas;
        
        public void startTimer()
        {
            timeRemaining = 120;
            isRunning = true;
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
                    Debug.Log("Time has run out!");
                    timeRemaining = 0;
                    isRunning = false;
                    EventSystemManager.OnTimeUp();
                    // todo: stop clock arm as well
                }
            }
        }
    }
}