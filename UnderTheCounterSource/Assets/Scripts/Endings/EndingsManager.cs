using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Technical;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SavedGameData;

namespace Endings
{
    public class EndingsManager : MonoBehaviour
    {
        public FadeCanvas endingCanvas;
        public Button continueButton;
        public Button creditsButton;
        public Image journal;
        public TextMeshProUGUI currentText;
        private List<Ending> _endingsText;
        private List<string> stringList;

        private string endingType;

        private void Start()
        {
            endingCanvas.FadeIn();

            continueButton.gameObject.SetActive(false);
            creditsButton.gameObject.SetActive(false);
            currentText.gameObject.SetActive(false);

            LoadEnding(GameData.loseType);
        }

        public void LoadEnding(string ending)
        {
            endingType = ending;
            string jsonString = Resources.Load<TextAsset>("TextAssets/EndingsData/Endings").text;
            _endingsText = JsonConvert.DeserializeObject<List<Ending>>(jsonString).FindAll(ending => ending.type == endingType);
            stringList = _endingsText[0].lines;

            journal.sprite = Resources.Load<Sprite>("Sprites/Endings/" + endingType);

            // used for debug info
            Debug.Log("Ending loaded: " + endingType);
            
            // Achievements
            StartCoroutine(CheckAchievements(ending));

            StartEndingText();
        }

        public void StartEndingText()
        {
            StartCoroutine(ShowText());
        }

        public IEnumerator ShowText()
        {
            yield return new WaitForSeconds(0.5f);
            currentText.text = stringList[0];
            currentText.GetComponent<FadeCanvas>().FadeIn();
            stringList.RemoveAt(0);
            if (stringList.Count > 0) 
            {
                StartCoroutine(ShowContinueButton());
            }
            else
            {
                StartCoroutine(ShowReturnButton());
            }
        }

        public IEnumerator ShowContinueButton()
        {
            yield return new WaitForSeconds(2f);
            continueButton.GetComponent<FadeCanvas>().FadeIn();
        }

        public void Continue()
        {
            StartCoroutine(OnContinuePressed());
        }

        public IEnumerator OnContinuePressed()
        {  
            continueButton.gameObject.SetActive(false);
            currentText.GetComponent<FadeCanvas>().FadeOut();
            yield return new WaitForSeconds(1f);
            StartCoroutine(ShowText());
        }

        public IEnumerator ShowReturnButton()
        {
            yield return new WaitForSeconds(2f);
            creditsButton.GetComponent<FadeCanvas>().FadeIn();
        }

        public void LoadCredits()
        {
            StartCoroutine(LoadCreditsScene());
        }

        public IEnumerator LoadCreditsScene()
        {
            endingCanvas.FadeOut();
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("CreditsScene");
        }

        private IEnumerator CheckAchievements(string ending)
        {
            yield return new WaitForSeconds(1f);
            
            if (ending == "blitz")
            {
                EventSystemManager.OnBlitzLose();
            }
            else if (ending == "bankrupt")
            {
                EventSystemManager.OnBankrupt();
            }
            else if (ending == "mafia")
            {
                EventSystemManager.OnBarBurned();
            }
            else if (ending == "good")
            {
                EventSystemManager.OnWin();
            }
        }
    }
}