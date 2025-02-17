using Bar;
using MasterBook;
using SavedGameData;
using Technical;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Settings
{
    public class InGameMenu : MonoBehaviour
    {
        [SerializeField] private MasterBookManager masterBook;
        private GameObject backToMainMenuDialog;
        private GameObject creditsDialog;
    
        private void Awake()
        {
            backToMainMenuDialog = transform.Find("BackToMainMenuDialog").gameObject;
            creditsDialog = transform.Find("CreditsDialog").gameObject;
        }

        private void OnEnable()
        {
            EventSystemManager.OnGamePaused?.Invoke();
        }

        private void OnDisable()
        {
            EventSystemManager.OnGameResumed?.Invoke();
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

        public void SaveGameShortcut()
        {
            GameData.SaveToJson();
        }
    }
}
