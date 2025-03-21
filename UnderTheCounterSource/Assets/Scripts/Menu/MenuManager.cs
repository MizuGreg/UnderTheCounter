using System.Collections;
using System.IO;
using Bar;
using SavedGameData;
using Technical;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup mainMenuCanvas;
        [SerializeField] private Button continueButton;
        [SerializeField] private FadeCanvas startGameConfirmationDialog;
    
        void Start()
        {
            mainMenuCanvas.GetComponent<FadeCanvas>().FadeIn();
            EventSystemManager.OnLoadMainMenu();
            SetContinueButtonVisible();
        }

        public void SetContinueButtonVisible()
        {
            continueButton.interactable = IsSaveFilePresent(); // hides continue button if there's no save file present
        }

        public void StartNewGameWithConfirmation()
        {
            if (IsSaveFilePresent()) startGameConfirmationDialog.FadeIn();
            else StartNewGame();
        }
        
        private bool IsSaveFilePresent()
        {
            return PlayerPrefs.HasKey("Save");
        }
        
        public void StartNewGame()
        {
            GameData.DeleteSave();
            GameData.Initialize();
            mainMenuCanvas.GetComponent<FadeCanvas>().FadeOut();
            StartCoroutine(WaitBeforeNewGame());
        }

        private IEnumerator WaitBeforeNewGame()
        {
            yield return new WaitForSeconds(1.1f);
            SceneManager.LoadScene("IntroductionScreen");
        }

        public void ContinueGame()
        {
            GameData.LoadFromJson();
            mainMenuCanvas.GetComponent<FadeCanvas>().FadeOut();
            StartCoroutine(WaitBeforeContinueGame());
        }

        private IEnumerator WaitBeforeContinueGame()
        {
            yield return new WaitForSeconds(1.1f);
            SceneManager.LoadScene("Mailbox");
        }
    
        public void QuitGame()
        {
            Technical.QuitGame.Quit();
        }
    }
}
