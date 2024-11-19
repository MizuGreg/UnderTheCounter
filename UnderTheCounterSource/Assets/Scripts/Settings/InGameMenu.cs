using Bar;
using Technical;
using UnityEngine;

namespace Settings
{
    public class InGameMenu : MonoBehaviour
    {
        private TimerManager _timerManager;
        private GameObject _quitGameDialog;
        private GameObject _settingsDialog;
    
        private void Awake()
        {
            _timerManager = GameObject.FindWithTag("BarManager").transform.GetComponent<TimerManager>();
            _quitGameDialog = transform.Find("QuitGameDialog").gameObject;
            _settingsDialog = transform.Find("SettingsDialog").gameObject;
        }

        private void OnEnable()
        {
            if (_timerManager != null) _timerManager.pauseTimer();
        }

        private void OnDisable()
        {
            if (_timerManager != null) _timerManager.resumeTimer();
        }

        public void OnEscapeButtonPressed()
        {
            if (!gameObject.activeSelf) GetComponent<FadeCanvas>().FadeIn();
            else if (_quitGameDialog.activeSelf) _quitGameDialog.GetComponent<FadeCanvas>().FadeOut();
            else if (_settingsDialog.activeSelf) _settingsDialog.GetComponent<FadeCanvas>().FadeOut();
            else GetComponent<FadeCanvas>().FadeOut();
        }
    }
}
