using Bar;
using Technical;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Settings
{
    public class InGameMenu : MonoBehaviour
    {
        private TimerManager _timerManager;
        [SerializeField] private RecipeBookManager recipeBookManager;
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
            if (!context.performed) return;
            if (!gameObject.activeSelf)
            {
                if (recipeBookManager.isRecipeBookOpen()) recipeBookManager.CloseRecipeBook();
            }
            else if (backToMainMenuDialog.activeSelf) backToMainMenuDialog.GetComponent<FadeCanvas>().FadeOut();
            else if (creditsDialog.activeSelf) creditsDialog.GetComponent<FadeCanvas>().FadeOut();
            else GetComponent<FadeCanvas>().FadeOut();
        }
    }
}
