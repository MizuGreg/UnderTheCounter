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
        public float letterDelay = 0.1f;
        public float phraseDelay = 2f;

        void Start()
        {
            thanksForPlayingCanvas.FadeIn();
            toBeContinuedCanvas.gameObject.SetActive(false);
            backToMenuButton.gameObject.SetActive(false);

            textComponent.gameObject.SetActive(false);

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
            textComponent.text = ""; // Inizializza il testo vuoto
            foreach (string phrase in phrases)
            {
                yield return StartCoroutine(DisplayTextOneByOne(phrase));
                yield return new WaitForSeconds(phraseDelay);
            }
            backToMenuButton.gameObject.SetActive(true);
        }

        private IEnumerator DisplayTextOneByOne(string fullText)
        {
            string currentText = textComponent.text; // Mantieni il testo attuale
            foreach (char c in fullText)
            {
                textComponent.text = currentText + c; // Aggiorna il testo progressivamente
                yield return new WaitForSeconds(letterDelay);
            }
            textComponent.text += "\n"; // Aggiungi un'interruzione di riga alla fine della frase
        }

        public void LoadMainMenu()
        {
            Debug.Log("Pulsante premuto! Avvio Coroutine...");
            StartCoroutine(LoadMainMenuScene());
        }

        public IEnumerator LoadMainMenuScene()
        {
            Debug.Log("Inizio transizione verso MainMenu...");
            toBeContinuedCanvas.FadeOut();
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("MainMenu");
        }
    }
}