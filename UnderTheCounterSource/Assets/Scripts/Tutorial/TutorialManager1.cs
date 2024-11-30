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
    public class TutorialManager1 : MonoBehaviour
    {
        [SerializeField] private CanvasGroup customerCanvas;
        [SerializeField] private Sprite ernestSprite;
        
        [Range(0.1f, 5f)]
        public float timeBeforeDialogue = 1f;
        
        private DialogueManager _dialogueManager;
        private Image _currentImage;
        private List<List<string>> _steps;
        private List<string> _currentStep;
        private int _actualStep = -1;
        
        private void Start()
        {
            EventSystemManager.NextTutorialStep += NextStep;
            
            _currentImage = customerCanvas.transform.Find("CustomerSprite").gameObject.GetComponent<Image>();
        }

        private void OnDestroy()
        {
            EventSystemManager.NextTutorialStep -= NextStep;
        }

        public void AttachDialogueManager(DialogueManager dialogueManager)
        {
            this._dialogueManager = dialogueManager;
        }


        public void StartTutorial()
        {
            Debug.Log("Tutorial 1 started");
            LoadSteps();
            NextStep();
        }
        
        private void LoadSteps()
        {
            // read Tutorial Steps json and create steps list
            #if UNITY_EDITOR
            string jsonString = File.ReadAllText("Assets/Data/TutorialData/Tutorial1.json");
            #else
            string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/TutorialData/Tutorial1.json");
            #endif
            
            //_steps = JsonConvert.DeserializeObject<CustomerList>(jsonString).customers;
            _steps = JsonConvert.DeserializeObject<List<List<string>>>(jsonString);
            
        }

        // this function will be used to proceed to the next step of the tutorial
        private void NextStep()
        {
            _actualStep++;
            
            switch (_actualStep)
            {
                case 0: Step0();
                    break;
                case 1: Step1();
                    break;
                case 2: Step2();
                    break;
                case 3: Step3();
                    break;
                default:
                    break;
            }
        }

        // In Step0 Ernest will pop in with an introductory message
        private void Step0()
        {
            Debug.Log("Step 0");
            //_currentStep = _steps[_actualStep];
            _currentStep = _steps[0];
            
            _currentImage.sprite = ernestSprite;
            customerCanvas.GetComponent<FadeCanvas>().FadeIn();
            StartCoroutine(WaitAndGreetDialogue());
            EventSystemManager.OnCustomerEnter();
        }

        // Ernest explains the CCA to the player
        private void Step1()
        {
            Debug.Log("Step 1");
            _currentStep = _steps[1];
            
            // CCA slides in 
            EventSystemManager.OnMakeCocktail(CocktailType.Wrong);
            
            // CCA becomes not interactable
            EventSystemManager.MakeIngredientInteractable(IngredientType.Unspecified);

            // Ernest pop up message
            StartCoroutine(WaitAndPopUp());
        }

        // Ernest returns to the dialogue view and tells the player to make their first cocktail
        private void Step2()
        {
            Debug.Log("Step 2");
            _currentStep = _steps[2];
            
            // CCA slides out
            EventSystemManager.HideCCA();
            
            // Ernest asks to make a Ripple
            _currentImage.sprite = ernestSprite;
            customerCanvas.GetComponent<FadeCanvas>().FadeIn();
            StartCoroutine(WaitAndGreetDialogue());
            EventSystemManager.OnCustomerEnter();
        }

        // Shows the order post-it
        private void Step3()
        {
            Debug.Log("Step 3");
            _currentStep = _steps[3];
            
            // CCA slides in
            EventSystemManager.OnMakeCocktail(CocktailType.Ripple);
            
            // Ernest pop up message
            StartCoroutine(WaitAndPopUp());
            
        }
        
        private IEnumerator WaitAndGreetDialogue()
        {
            yield return new WaitForSeconds(timeBeforeDialogue);
            
            _dialogueManager.StartDialogue(
                new Dialogue("Ernest", _currentStep),
                DialogueType.Tutorial);
            
        }

        private IEnumerator WaitAndPopUp()
        {
            yield return new WaitForSeconds(timeBeforeDialogue);
            
            _dialogueManager.StartPopUp(new Dialogue("Ernest", _currentStep));
            
        }

        private void StartPopUp()
        {
            _dialogueManager.StartPopUp(new Dialogue("Ernest", _currentStep));
        }
        
        
        
        
        // Collegamenti:
        // - bottone startDay -> chiama StartDay() in BarManager
        // - StartDay() lancia l'evento -> ascoltato da TutorialManager1
        // - tutorial che fa le sue cose
        // - TutorialManager chiama l'evento OnTutorial1End -> ascoltato da CustomerManager
        
        // Altri collegamenti utili:
        // - StartDay() in BarManager -> chiama StartDay() in CustomerManager
        // - CustomerManager -> DialogueManager
        // - DialogueManager -> CustomerManager -> CocktailManager
        // - CocktailScript -> chiama OnGarnishAdded che è ascoltato da CocktailManager e chiama ServeCocktail
        // - ServeCocktail -> chiama OnCocktailMade che è ascoltato da CustomerManager che chiama ServeCustomer()
        // - ServeCustomer -> chiama DialogueManager
        
        // attualmente, i dialoghi vengono gestiti dal dialogue manager che viene chiamato da customer manager
        // Nella funzione WaitAndGreetDialogue
        // NB: bisogna settare la posizione dei pop-up
        
        // quando devi servire il cocktail chiama l'evento OnMakeCocktail(CocktailType.Everest)
        
        // quando finisce il tutorial 1 deve chiamare l'evento OnTutorial1End
        
        
        // NB: quando fai slideIn della CCA, se chiami OnMakeCocktail con l'enum del wrong cocktail non viene mostrato il post it
        //     (utile per i primi step del tutorial)
    }
}
