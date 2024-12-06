using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Technical;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Endings
{
    public class VictoryScreenManager : MonoBehaviour
    {
        public FadeCanvas thanksForPlayingCanvas;
        public FadeCanvas toBeContinuedCanvas;
        public Button backToMenuButton;
        public TextMeshProUGUI textComponent;
        public string[] phrases;
        public float letterDelay;
        public float phraseDelay;
        // public float lineSpacing;

        void Start()
        {
            EventSystemManager.OnLoadLoseScreen();
            
            thanksForPlayingCanvas.FadeIn();
            toBeContinuedCanvas.gameObject.SetActive(false);
            backToMenuButton.gameObject.SetActive(false);

            textComponent.gameObject.SetActive(false);
            // textComponent.lineSpacingAdjustment = lineSpacing;

            StartCoroutine(StartTimeBetweenCanvases());
        }

        public IEnumerator StartTimeBetweenCanvases()
        {
            yield return new WaitForSeconds(4f);
            thanksForPlayingCanvas.FadeOut();
            yield return new WaitForSeconds(2f);
            toBeContinuedCanvas.FadeIn();
            yield return new WaitForSeconds(2f);
            
            StartCoroutine(DisplayPhrases());
        }

        
        public IEnumerator DisplayPhrases()
        {
            textComponent.gameObject.SetActive(true);
            textComponent.text = ""; 
            foreach (string phrase in phrases)
            {
                Debug.Log("Sto scrivendo: " + phrase); // Debug
                yield return StartCoroutine(DisplayTextOneByOne(phrase));
                yield return new WaitForSeconds(phraseDelay);
            }
            backToMenuButton.gameObject.SetActive(true);
        }

        private IEnumerator DisplayTextOneByOne(string fullText)
        {
            foreach (char c in fullText)
            {
                textComponent.text += c; 
                yield return new WaitForSeconds(letterDelay);
            }
            textComponent.text += "<line-height=375%>\n"; 
        }

        public void LoadMainMenu()
        {
            Debug.Log("Pulsante premuto! Avvio Coroutine..."); // Debug
            StartCoroutine(LoadMainMenuScene());
        }

        public IEnumerator LoadMainMenuScene()
        {
            Debug.Log("Inizio transizione verso MainMenu..."); // Debug
            toBeContinuedCanvas.FadeOut();
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("MainMenu");
        }
    }
}