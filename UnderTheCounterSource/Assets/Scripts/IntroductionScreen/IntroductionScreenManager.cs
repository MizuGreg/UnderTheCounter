using System.Collections;
using SavedGameData;
using Technical;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IntroductionScreen
{
    public class IntroductionScreenManager : MonoBehaviour
    {
        [Header("Canvas Groups")]
        [SerializeField] private CanvasGroup mainCanvas;
        [SerializeField] private CanvasGroup continueButtonCanvas;
        [SerializeField] private CanvasGroup startDayButtonCanvas;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI text1;
        [SerializeField] private TextMeshProUGUI text2;
        [SerializeField] private TextMeshProUGUI text3;

        private int currentStep = 0; // 0 -> Mostra text1, 1 -> Mostra text2, 2 -> Mostra text3
        private float buttonFadeDuration = 0.6f;
        private float textFadeDuration = 0.8f;

    private void Start()
    {
        // Inizialmente disattivo i pulsanti e l'input field
        continueButtonCanvas.alpha = 0;
        continueButtonCanvas.gameObject.SetActive(false);

        startDayButtonCanvas.alpha = 0;
        startDayButtonCanvas.gameObject.SetActive(false);
        
        text1.alpha = 0;
        text2.alpha = 0;
        text3.alpha = 0;

        StartCoroutine(StartSequence());
    }

        private IEnumerator StartSequence()
        {
            mainCanvas.alpha = 0;
            // Fade-in scena principale
            yield return FadeCanvasGroupIn(mainCanvas, 1f);

            // Fade-in text1
            yield return FadeTextIn(text1, textFadeDuration);

            // Fade-in pulsante continue
            continueButtonCanvas.gameObject.SetActive(true);
            yield return FadeCanvasGroupIn(continueButtonCanvas, buttonFadeDuration);
        }

        public void OnContinuePressed()
        {
            StartCoroutine(ContinueSequence());
        }

        private IEnumerator ContinueSequence()
        {
            if (currentStep == 0) 
            {
                // Fade-out text1 e pulsante continue
                yield return FadeCanvasGroupOut(continueButtonCanvas, buttonFadeDuration);
                continueButtonCanvas.gameObject.SetActive(false);

                yield return FadeTextOut(text1, textFadeDuration);

                // Fade-in text2
                yield return FadeTextIn(text2, textFadeDuration);

                // Fade-in pulsante continue
                continueButtonCanvas.gameObject.SetActive(true);
                yield return FadeCanvasGroupIn(continueButtonCanvas, buttonFadeDuration);

                currentStep = 1;
            }
            else if (currentStep == 1)
            {
                // Fade-out text2 e pulsante continue
                yield return FadeCanvasGroupOut(continueButtonCanvas, buttonFadeDuration);
                continueButtonCanvas.gameObject.SetActive(false);

                yield return FadeTextOut(text2, textFadeDuration);

                // Fade-in text3
                yield return FadeTextIn(text3, textFadeDuration);
                
                yield return new WaitForSeconds(1f); // extra waiting time before the final button

                // Fade-in pulsante start day
                startDayButtonCanvas.gameObject.SetActive(true);
                yield return FadeCanvasGroupIn(startDayButtonCanvas, buttonFadeDuration);

                currentStep = 2;
            }
        }

        public void OnStartDayPressed()
        {
            StartCoroutine(LoadNextScene());
        }

        private IEnumerator LoadNextScene()
        {
            // Se vuoi un fade-out del mainCanvas prima del cambio scena:
            yield return FadeCanvasGroupOut(mainCanvas, 1.1f);

            SceneManager.LoadScene("Scenes/TutorialDay1");
        }

        // ----- FUNZIONI UTILI PER IL FADE -----

        private IEnumerator FadeTextIn(TextMeshProUGUI text, float duration)
        {
            text.gameObject.SetActive(true);
            float startAlpha = text.alpha;
            float endAlpha = 1f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                text.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
                yield return null;
            }
        }

        private IEnumerator FadeTextOut(TextMeshProUGUI text, float duration)
        {
            float startAlpha = text.alpha;
            float endAlpha = 0f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                text.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
                yield return null;
            }

            text.gameObject.SetActive(false);
        }

        private IEnumerator FadeCanvasGroupIn(CanvasGroup cg, float duration)
        {
            cg.interactable = false;
            float startAlpha = cg.alpha;
            float endAlpha = 1f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                cg.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
                yield return null;
            }
            cg.interactable = true;
        }

        private IEnumerator FadeCanvasGroupOut(CanvasGroup cg, float duration)
        {
            cg.interactable = false;
            float startAlpha = cg.alpha;
            float endAlpha = 0f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                cg.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
                yield return null;
            }
        }
        
        public void BackToMainMenu()
        {
            mainCanvas.GetComponent<FadeCanvas>().FadeOut();
            StartCoroutine(WaitBeforeMenu());
        }
        
        private IEnumerator WaitBeforeMenu()
        {
            yield return new WaitForSeconds(1.1f);
            SceneManager.LoadScene("MainMenu");
        }
    }
}
