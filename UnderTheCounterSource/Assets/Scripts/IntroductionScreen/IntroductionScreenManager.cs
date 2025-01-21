using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SavedGameData;
using Technical;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IntroductionScreen
{
    public class IntroductionScreenManager : MonoBehaviour
    {
        [Header("Canvas Groups")]
        [SerializeField] private CanvasGroup mainCanvas;
        [SerializeField] private CanvasGroup continueButtonCanvas;
        [SerializeField] private CanvasGroup startDayButtonCanvas;
        [SerializeField] private CanvasGroup containerCanvas;
        [SerializeField] private CanvasGroup imageCanvas;

        [Header("Slide Elements")] 
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI text;
        private float typingSpeed = 0.05f;
        
        private List<Slide> _slides;
        private CanvasGroup _currentButtonCanvas;
        
        private string _fullText;
        private string _currentText = "";
        private Coroutine _typingCoroutine;
        private bool _isTyping = false;


        private void Start()
        {
            // Initializations
            _currentButtonCanvas = continueButtonCanvas;
            mainCanvas.alpha = 0f;
            imageCanvas.alpha = 0f;
            text.alpha = 0f;
            continueButtonCanvas.alpha = 0f;
            continueButtonCanvas.blocksRaycasts = false;
            startDayButtonCanvas.alpha = 0f;
            startDayButtonCanvas.blocksRaycasts = false;
            
            // Load Slides infos from the JSON
            LoadSlides();
            
            StartCoroutine(DisplayFirstSlide());
        }
        
        private void LoadSlides()
        {
            // Read Introduction JSON and create slides list
            string jsonString = Resources.Load<TextAsset>("TextAssets/IntroductionData/Introduction").text;
            _slides = JsonConvert.DeserializeObject<SlideList>(jsonString).slides;
        }

        private IEnumerator DisplayFirstSlide()
        {
            yield return FadeCanvasGroupIn(mainCanvas, 1.1f);
            
            // Display first slide
            NextSlide();
            
            // Progressive Fade-in
            yield return DisplaySlideContent();
        }

        private IEnumerator DisplaySlideContent()
        {
            // Fade-in (Image)
            yield return FadeCanvasGroupIn(imageCanvas, 1.1f);
            
            // Type text (Text)
            _typingCoroutine = StartCoroutine(TypeText());

            // Non mostrare subito il pulsante
        }
        
        private IEnumerator TypeText()
        {
            _isTyping = true;
            _currentText = ""; 
            text.text = _currentText;
            text.alpha = 1f;

            foreach (char c in _fullText)
            {
                if (!_isTyping)
                {
                    // Se il typing viene interrotto, esci e mostra il testo completo
                    _currentText = _fullText;
                    text.text = _currentText;
                    break;
                }

                _currentText += c;
                text.text = _currentText;
                yield return new WaitForSeconds(typingSpeed);
            }

            _isTyping = false;

            // Mostra il pulsante alla fine del typing
            _currentButtonCanvas.alpha = 1f;
            _currentButtonCanvas.blocksRaycasts = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonUp(0))
            {
                if (_isTyping)
                {
                    // Interrompi il typing e mostra immediatamente tutto il testo
                    _isTyping = false;
                    if (_typingCoroutine != null)
                    {
                        StopCoroutine(_typingCoroutine);
                    }
                    _currentText = _fullText;
                    text.text = _currentText;

                    // Mostra il pulsante Continue
                    _currentButtonCanvas.alpha = 1f;
                    _currentButtonCanvas.blocksRaycasts = true;
                }

                // else if (_currentButtonCanvas.blocksRaycasts)
                // {
                    // Se il typing è già finito, comportati come se fosse stato premuto "Continue"
                    // Continue();
                // }
            }
        }

        public void Continue()
        {
            _currentButtonCanvas.blocksRaycasts = false;
            
            StartCoroutine(DisplaySlide());
        }

        private IEnumerator DisplaySlide()
        {
            // Fade-out (Image, Text, Button)
            yield return FadeCanvasGroupOut(containerCanvas, 1.1f);

            imageCanvas.alpha = 0f;
            text.alpha = 0f;
            _currentButtonCanvas.alpha = 0f;

            containerCanvas.alpha = 1f;
            containerCanvas.interactable = true;
            
            // Load next slide content
            NextSlide();

            // Progressive Fade-in
            yield return DisplaySlideContent();
        }

        private void NextSlide()
        {
            if (_slides.Count > 0)
            {
                image.sprite = GetSpriteFromSlide(_slides[0].sprite);
                _fullText = _slides[0].caption;
                _slides.RemoveAt(0);

                if (_slides.Count == 0)
                {
                    _currentButtonCanvas.gameObject.SetActive(false);
                    _currentButtonCanvas = startDayButtonCanvas;
                    _currentButtonCanvas.gameObject.SetActive(true);
                }
            }
        }
        
        private Sprite GetSpriteFromSlide(string sprite)
        {
            try
            {
                return Resources.Load("Sprites/Introduction/" + sprite, typeof(Sprite)) as Sprite;
            }
            catch (Exception e)
            {
                print($"Exception in getSprite: {e}");
                return Resources.Load("Sprites/Introduction/Intro 1_Letter", typeof(Sprite)) as Sprite;
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
