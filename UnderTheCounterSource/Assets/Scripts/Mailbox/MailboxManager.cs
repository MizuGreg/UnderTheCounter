using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SavedGameData;
using Technical;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Mailbox
{
    public class MailboxManager : MonoBehaviour
    {
        [Header("Mail GameObject")]
        [SerializeField] private GameObject mail;

        [Header("Carousel Stuff")] 
        [SerializeField] private CanvasGroup blackBG;
        [SerializeField] private CanvasGroup element;
        [SerializeField] private CanvasGroup nextButton;
        
        private List<Mailbox> _mailboxes;
        private Mailbox _currentMailbox;

        private List<string> _carouselElements = new List<string>();
        private int _currentElement = 0;
        
        private void Start()
        {
            LoadMailbox();

            mail.GetComponent<Image>().sprite = GetMailFromDay(_currentMailbox.day);
            mail.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.01f;
            
            SetCarouselElements();
        }
        
        private Sprite GetMailFromDay(int day)
        {
            try
            {
                return Resources.Load("Sprites/Mailbox/Day " + day + "/Mask group_Day" + day, typeof(Sprite)) as Sprite;
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
            //_currentMailbox = _mailboxes.Find(a => a.day == GameData.CurrentDay);
            // DEBUG
            _currentMailbox = _mailboxes.Find(a => a.day == 3);
        }
        
        
        private void SetCarouselElements()
        {
            if(_currentMailbox.vote) _carouselElements.Add("Propaganda poster_Day 5");
            if(_currentMailbox.newspaper) _carouselElements.Add($"newspaper_Day{_currentMailbox.day}");
            if(_currentMailbox.theater) _carouselElements.Add("Margaret poster_Day 4");
            if (_currentMailbox.BU) _carouselElements.Add($"BU_Day {_currentMailbox.day}");
            if(_currentMailbox.letter) _carouselElements.Add($"Letter_Day{_currentMailbox.day}");
        }

        public void OpenCarousel()
        {
            StartCoroutine(FadeCanvasGroup(blackBG, 1.1f, 0.4f));
            blackBG.blocksRaycasts = true;
            
            element.gameObject.GetComponent<Image>().sprite = Resources.Load("Sprites/Mailbox/Day " + _currentMailbox.day + "/"+_carouselElements[_currentElement], typeof(Sprite)) as Sprite;
            StartCoroutine(FadeCanvasGroup(element, 1.1f, 1f));

            StartCoroutine(FadeCanvasGroup(nextButton, 1.1f, 1f));
            nextButton.blocksRaycasts = true;
            nextButton.interactable = true;
        }

        public void NextCarouselItem()
        {
            if (_currentElement < _carouselElements.Count - 1)
            {
                _currentElement++;
                element.gameObject.GetComponent<Image>().sprite = Resources.Load("Sprites/Mailbox/Day " + _currentMailbox.day + "/"+_carouselElements[_currentElement], typeof(Sprite)) as Sprite;
                
                // This is for the sound effect
                EventSystemManager.OnMasterBookOpened();
            }
            else
            {
                DeactivateElement(nextButton);
            }
        }

        private IEnumerator ElementTransition()
        {
            if (_currentElement < _carouselElements.Count - 1)
            {
                yield return FadeCanvasGroup(element, 1.1f, 0f);
            
                // This is for the sound effect
                EventSystemManager.OnMasterBookOpened();
                
                _currentElement++;
                element.gameObject.GetComponent<Image>().sprite = Resources.Load("Sprites/Mailbox/Day " + _currentMailbox.day + "/"+_carouselElements[_currentElement], typeof(Sprite)) as Sprite;

                yield return FadeCanvasGroup(element, 1.1f, 1f);
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
        
    }
}
