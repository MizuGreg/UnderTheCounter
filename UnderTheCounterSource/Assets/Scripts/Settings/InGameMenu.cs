using Bar;
using MasterBook;
using Technical;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Settings
{
    public class InGameMenu : MonoBehaviour
    {
        private TimerManager _timerManager;
        [SerializeField] private MasterBookManager masterBook;
        private GameObject backToMainMenuDialog;
        private GameObject creditsDialog;
    
        private void Awake()
        {
            _timerManager = GameObject.FindWithTag("BarManager")?.transform.GetComponent<TimerManager>();
            
            backToMainMenuDialog = transform.Find("BackToMainMenuDialog").gameObject;
            creditsDialog = transform.Find("CreditsDialog").gameObject;
        }

        private void OnEnable()
        {
            if (_timerManager != null) _timerManager.PauseTimer();
        }

        private void OnDisable()
        {
            if (_timerManager != null) _timerManager.ResumeTimer();
        }

        public void OnEscapeButtonPressed(InputAction.CallbackContext context)
        {
            // In order: closes back to main menu confirmation dialog
            // else closes credits dialog
            // else closes in-game menu
            // else closes recipe book
            // else opens in-game menu
            
            if (backToMainMenuDialog != null && backToMainMenuDialog.activeSelf) backToMainMenuDialog.GetComponent<FadeCanvas>().FadeOut();
            
            else if (creditsDialog != null &&creditsDialog.activeSelf) creditsDialog.GetComponent<FadeCanvas>().FadeOut();
            
            else if (gameObject.activeSelf) GetComponent<FadeCanvas>().FadeOut();
            
            else if (masterBook != null && masterBook.IsMasterBookOpen()) masterBook.CloseMasterBook();
            
            else GetComponent<FadeCanvas>().FadeIn();
        }
    }
}
