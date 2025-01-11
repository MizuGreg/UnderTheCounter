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

namespace Endings
{
    public class EndingsManager : MonoBehaviour
    {
        public FadeCanvas endingCanvas;
        public Button continueButton;
        public Button backToMenuButton;
        public Image journal;
        public TextMeshProUGUI currentText;
        private List<Ending> _endingsText;
        private List<string> stringList;

        private void Start()
        {
            EventSystemManager.OnLoadLoseScreen += LoadEnding;
            
            continueButton.gameObject.SetActive(false);
            backToMenuButton.gameObject.SetActive(false);
            currentText.gameObject.SetActive(false);

            // used for testing
            LoadEnding("mafia");
        }

        public void LoadEnding(string endingType)
        {
            string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/EndingsData/Endings.json");
            _endingsText = JsonConvert.DeserializeObject<List<Ending>>(jsonString).FindAll(ending => ending.type == endingType);
            stringList = _endingsText[0].lines;

            journal.sprite = Resources.Load<Sprite>("Sprites/Endings/" + endingType);

            FadeEnding();
        }

        public void FadeEnding()
        {
            endingCanvas.FadeIn();
            StartCoroutine(ShowText());
        }

        public IEnumerator ShowText()
        {
            yield return new WaitForSeconds(1f);
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
            backToMenuButton.GetComponent<FadeCanvas>().FadeIn();
        }

        public void LoadMainMenu()
        {
            StartCoroutine(LoadMainMenuScene());
        }

        public IEnumerator LoadMainMenuScene()
        {
            endingCanvas.FadeOut();
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("MainMenu");
        }
    }
}