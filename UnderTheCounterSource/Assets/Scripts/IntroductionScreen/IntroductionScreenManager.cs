using System;
using System.Collections;
using System.Collections.Generic;
using Technical;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace IntroductionScreen
{
    public class IntroductionScreenManager : MonoBehaviour {

        [SerializeField] private CanvasGroup mainCanvas;
        [SerializeField] private CanvasGroup startDayButtonCanvas;
        
        void Start() {
            mainCanvas.GetComponent<FadeCanvas>().FadeIn();
            StartCoroutine(ShowButton());
        }

        private IEnumerator ShowButton() {
            startDayButtonCanvas.gameObject.SetActive(false);
            yield return new WaitForSeconds(3f);
            startDayButtonCanvas.GetComponent<FadeCanvas>().FadeIn();
        }

        public void StartDay() {
            StartCoroutine(WaitThenStartDay());
        }

        private IEnumerator WaitThenStartDay() {
            mainCanvas.GetComponent<FadeCanvas>().FadeOut();
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene("Scenes/BarView_Timer");
        }
    }
}