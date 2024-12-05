using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Bar;
using CocktailCreation;
using Technical;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialManager2 : MonoBehaviour
    {
        private DialogueManager _dialogueManager;
        private List<List<string>> _steps;
        private List<string> _currentStep;
        private int _actualStep = -1;

        private void Awake()
        {
            // enabled = Day.CurrentDay == 2; // goes to sleep if it's not the second day
        }
        private void Start()
        {
            _dialogueManager = GetComponent<DialogueManager>();
            
            EventSystemManager.NextTutorialStep += NextStep;
        }

        private void OnDestroy()
        {
            EventSystemManager.NextTutorialStep -= NextStep;
        }
        


        public void StartTutorial()
        {
            Debug.Log("Tutorial 2 started");
            LoadSteps();
            NextStep();
        }
        
        private void LoadSteps()
        {
            // read Tutorial Steps json and create steps list
            string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/TutorialData/Tutorial2.json");
            
            //_steps = JsonConvert.DeserializeObject<CustomerList>(jsonString).customers;
            _steps = JsonConvert.DeserializeObject<List<List<string>>>(jsonString);
            
        }

        // this function will be used to proceed to the next step of the tutorial
        private void NextStep()
        {
            _actualStep++;
            _currentStep = _steps[_actualStep];
            
            switch (_actualStep)
            {
                case 0: Step0();
                    break;
                default: EndTutorial();
                    break;
            }
        }

        // In Step0 Ernest will pop in with an introductory message
        private void Step0()
        {
            StartCoroutine(WaitAndPopUp(false));
        }
        
        private void EndTutorial()
        {
            Debug.Log("Tutorial 2 ended");
        }

        private IEnumerator WaitAndPopUp(bool isActionNeeded)
        {
            yield return new WaitForSeconds(1f);
            
            _dialogueManager.StartPopUp(new Dialogue("Ernest", _currentStep), isActionNeeded);
            
        }

        public void EndPopUp()
        {
            _dialogueManager.EndPopUp();
        }
    }
}
