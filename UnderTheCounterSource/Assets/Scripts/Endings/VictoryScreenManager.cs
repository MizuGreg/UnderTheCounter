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
        [SerializeField] private FadeCanvas thanksForPlayingCanvas;
        [SerializeField] private FadeCanvas toBeContinuedCanvas;
        [SerializeField] private Button backToMenuButton;
        [SerializeField] private TextMeshProUGUI textComponent;
        [SerializeField] private string[] phrases;
        [SerializeField] private float letterDelay;
        [SerializeField] private float phraseDelay;

        [SerializeField] private Image[] arrows;

        private int currentArrow = 0;
        // public float lineSpacing;

        void Start()
        {
            EventSystemManager.OnLoadWinScreen();
            
            thanksForPlayingCanvas.FadeIn();
            toBeContinuedCanvas.gameObject.SetActive(false);
            backToMenuButton.gameObject.SetActive(false);

            textComponent.gameObject.SetActive(false);
            // textComponent.lineSpacingAdjustment = lineSpacing;

            StartCoroutine(StartTimeBetweenCanvases());

            foreach (Image arrow in arrows)
            {
                arrow.gameObject.SetActive(false);
            }
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
            yield return new WaitForSeconds(phraseDelay); // additional delay before displaying main menu button
            backToMenuButton.GetComponent<FadeCanvas>().FadeIn();
        }

        private IEnumerator DisplayTextOneByOne(string fullText)
        {
            arrows[currentArrow++].gameObject.SetActive(true);
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