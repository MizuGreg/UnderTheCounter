using System.Collections;
using SavedGameData;
using Technical;
using UnityEngine;
using UnityEngine.UI;

namespace Bar
{
    public class TimerManager : MonoBehaviour
    {
        private float initialTime;
        [SerializeField] private float timeRemaining;
        public bool isOn = false;
        public bool isRunning = false;
        private bool warningRinged = false;

        [SerializeField] private Image wallClockOutline;
        private Color warningColor = new(1f, 1f, 0.65f, 1f);
        private Color timeUpColor = new(1f, 0.25f, 0f, 1f);
        
        [SerializeField] private Image wallClockHour;
        [SerializeField] private Image wallClockMinute;
        private readonly float timeCoherenceRotation = 100f;

        public void SetTime()
        {
            initialTime = GameData.DailyTime;
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
                    if (timeRemaining < 30 && !warningRinged)
                    {
                        warningRinged = true;
                        StartCoroutine(BlinkOutline(warningColor));
                        EventSystemManager.OnTimeWarning();
                    }
                }
                else
                {
                    print("Time has run out!");
                    timeRemaining = 0;
                    isRunning = false;
                    StartCoroutine(BlinkOutline(timeUpColor));
                    EventSystemManager.OnTimeUp();
                }
            }
        }
        
        public void StartTimer()
        {
            timeRemaining = GameData.DailyTime;
            isOn = true;
            isRunning = true;
            print($"Timer started. Time remaining: {timeRemaining}");
        }

        public void PauseTimer()
        {
            if (!isOn) return;
            isRunning = false;
        }

        public void ResumeTimer()
        {
            if (!isOn) return;
            if (timeRemaining > 0) isRunning = true;
        }
        
        private IEnumerator BlinkOutline(Color outlineColor)
        {
            wallClockOutline.gameObject.SetActive(true);
            wallClockOutline.color = outlineColor;
            
            float fadeDuration = 0.4f;
            
            // blinks 5 times
            for (int i = 1; i <= 5; i++)
            {
               yield return StartCoroutine(FadeAlpha(wallClockOutline, 0f, 1f, fadeDuration));
               yield return StartCoroutine(FadeAlpha(wallClockOutline, 1f, 0f, fadeDuration)); 
            }
            
            wallClockOutline.gameObject.SetActive(false);
        }

        private IEnumerator FadeAlpha(Image img, float startAlpha, float endAlpha, float duration)
        {
            float elapsed = 0f;
            Color c = img.color;
            while(elapsed < duration)
            {
                float t = elapsed / duration;
                c.a = Mathf.Lerp(startAlpha, endAlpha, t);
                img.color = c;
                elapsed += Time.deltaTime;
                yield return null;
            }
            c.a = endAlpha;
            img.color = c;
        }
    }
}