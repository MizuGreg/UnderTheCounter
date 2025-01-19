using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SavedGameData;
using Technical;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mailbox
{
    public class MailboxManager : MonoBehaviour
    {
        [Header("Main and Container Canvas")] 
        [SerializeField] private CanvasGroup mainCanvas;
        [SerializeField] private CanvasGroup containerCanvas;
        
        [Header("Mail GameObject")]
        [SerializeField] private GameObject mail;

        [Header("Carousel Stuff")] 
        [SerializeField] private CanvasGroup blackBG;
        [SerializeField] private CanvasGroup element;
        [SerializeField] private CanvasGroup nextButton;
        [SerializeField] private CanvasGroup prevButton;
        [SerializeField] private CanvasGroup continueButton;
        
        [Header("Testing stuff")]
        [Range(1,7)]
        [SerializeField] private int forceDay;
        [SerializeField] private bool forceButterfly;
        
        private List<Mailbox> _mailboxes;
        private Mailbox _currentMailbox;

        private List<string> _carouselElements = new List<string>();
        private int _currentElement = 0;
        private bool _isButterfly;
        
        private void Start()
        {
            containerCanvas.alpha = 0f;
            
            LoadMailbox();

            mail.GetComponent<Image>().sprite = GetMailFromDay(_currentMailbox.day);
            mail.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.01f;
            
            StartCoroutine(FadeCanvasGroup(containerCanvas, 1.1f, 1f));
            
            SetCarouselElements();
        }
        
        private Sprite GetMailFromDay(int day)
        {
            try
            {
                string path = "Sprites/Mailbox/Day " + day + "/Mask group_Day" + day;
                
                if (_currentMailbox.day == 5 && _isButterfly)
                {
                    path = path + "_butterfly";
                }
                
                return Resources.Load(path, typeof(Sprite)) as Sprite;
            }
            catch (Exception e)
            {
                print($"Exception in getSprite: {e}");
                return Resources.Load("Sprites/Mailbox/Day 2/Mask group_Day2", typeof(Sprite)) as Sprite;
            }
        
        }

        private void LoadMailbox()
        {
            // Read Mailboxes JSON and create mailboxes list
            string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/MailboxData/Mailboxes.json");
            
            _mailboxes = JsonConvert.DeserializeObject<MailboxList>(jsonString).mailboxes;
            
            // Get the mailbox based on current day
            if (GameData.CurrentDay < 2) forceDay = 2;
            if (forceDay != 1)
            {
                _currentMailbox = _mailboxes.Find(a => a.day == forceDay);
            }
            else
            {
                _currentMailbox = _mailboxes.Find(a => a.day == GameData.CurrentDay);
            }
            
            // Check for butterfly effect
            if (forceButterfly)
            {
                _isButterfly = true;
            }
            else
            {
                _isButterfly = GameData.Choices["MargaretDrunk"];
            }
        }
        
        private void SetCarouselElements()
        {
            if(_currentMailbox.vote) _carouselElements.Add("Propaganda poster_Day 5");

            if (_currentMailbox.newspaper)
            {
                string newspaperPath = $"newspaper_Day{_currentMailbox.day}";
                if (_currentMailbox.day == 5 && _isButterfly)
                {
                    newspaperPath = newspaperPath + "_butterfly";
                }
                _carouselElements.Add(newspaperPath);
            }
            
            if(_currentMailbox.theater) _carouselElements.Add("Margaret poster_Day 4");
            if (_currentMailbox.BU) _carouselElements.Add($"BU_Day {_currentMailbox.day}");
            if(_currentMailbox.letter) _carouselElements.Add($"Letter_Day{_currentMailbox.day}");
        }

        public void OpenCarousel()
        {
            StartCoroutine(FadeCanvasGroup(blackBG, 1.1f, 0.4f));
            blackBG.blocksRaycasts = true;
            
            element.gameObject.GetComponent<Image>().sprite = Resources.Load("Sprites/Mailbox/Day " + _currentMailbox.day + "/"+_carouselElements[_currentElement], typeof(Sprite)) as Sprite;
            EventSystemManager.OnMasterBookOpened(); // This is for the sound effect
            StartCoroutine(FadeCanvasGroup(element, 1.1f, 1f));
            
            // Achievement
            if(_currentMailbox.day == 3)
            {
                EventSystemManager.OnBreakingNews();
            }

            StartCoroutine(FadeCanvasGroup(nextButton, 1.1f, 1f));
            nextButton.blocksRaycasts = true;
            nextButton.interactable = true;
        }

        public void NextCarouselItem(int step)
        {
            if (_currentElement + step >= 0 && _currentElement + step < _carouselElements.Count)
            {
                _currentElement += step;
                element.gameObject.GetComponent<Image>().sprite = Resources.Load("Sprites/Mailbox/Day " + _currentMailbox.day + "/"+_carouselElements[_currentElement], typeof(Sprite)) as Sprite;
                
                // This is for the sound effect
                EventSystemManager.OnMasterBookOpened();

                if (_currentElement == _carouselElements.Count - 1)
                {
                    DeactivateElement(nextButton);
                    ActivateElement(continueButton);
                }
                else
                {
                    ActivateElement(nextButton);
                }

                if (_currentElement == 0)
                {
                    DeactivateElement(prevButton);
                }
                else
                {
                    ActivateElement(prevButton);
                }
            }
        }

        private void ActivateElement(CanvasGroup cg)
        {
            cg.alpha = 1f;
            cg.blocksRaycasts = true;
            cg.interactable = true;
        }

        private void DeactivateElement(CanvasGroup cg)
        {
            cg.alpha = 0f;
            cg.blocksRaycasts = false;
            cg.interactable = false;
        }
        
        private IEnumerator FadeCanvasGroup(CanvasGroup cg, float duration, float finalAlpha)
        {
            cg.interactable = false;
            float startAlpha = cg.alpha;
            float endAlpha = finalAlpha;
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

        public void Continue()
        {
            StartCoroutine(LoadNextScene());
        }

        private IEnumerator LoadNextScene()
        {
            yield return FadeCanvasGroup(mainCanvas,1.1f,0f);
            SceneManager.LoadScene("ShopWindow");
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
