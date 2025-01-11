using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Bar;
using CocktailCreation;
using Technical;
using Newtonsoft.Json;
using SavedGameData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialManager1 : MonoBehaviour
    {
        [Header("Canvases")]
        [SerializeField] private CanvasGroup customerCanvas;
        [SerializeField] private CanvasGroup customerCocktail;
        
        [Header("Images")]
        [SerializeField] private Sprite ernestSprite;
        [SerializeField] private Image postIt;
        
        [Header("Ingredients")]
        [SerializeField] private CanvasGroup ingredientSquare;
        [SerializeField] private Image caledonImage;
        [SerializeField] private Image shaddockImage;
        [SerializeField] private Image gryteImage;
        [SerializeField] private Image shakerOutline;
        
        [Header("Buttons")]
        [SerializeField] private GameObject recipeBookIcon;
        [SerializeField] private Button recipeBookClosingIcon;
        [SerializeField] private Button mixButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button serveButton;
        [SerializeField] private Button trashButton;
        [SerializeField] private Button waterButton;
        
        [Range(0.1f, 5f)]
        public float timeBeforeDialogue = 1f;
        
        private DialogueManager _dialogueManager;
        private Image _currentImage;
        private List<List<string>> _steps;
        private List<string> _currentStep;
        private int _actualStep = -1;

        private GameObject outline;
        
        [SerializeField] private FadeCanvas nameYourBarCanvas;

        private void Awake()
        {
            enabled = GameData.CurrentDay == 1; // goes to sleep if it's not the first day
            // update: this line doesn't work...
        }
        private void Start()
        {
            EventSystemManager.NextTutorialStep += NextStep;
            EventSystemManager.OnMasterBookOpened += RecipeBookOpenedFirstTime;
            EventSystemManager.OnMasterBookClosed += RecipeBookClosedFirstTime;
            EventSystemManager.OnIngredientPoured += IngredientPoured;
            EventSystemManager.OnGarnishAdded += GarnishAdded;
            
            _currentImage = customerCanvas.transform.Find("CustomerSprite").gameObject.GetComponent<Image>();
            
        }

        private void OnDestroy()
        {
            EventSystemManager.NextTutorialStep -= NextStep;
            EventSystemManager.OnMasterBookOpened -= RecipeBookOpenedFirstTime;
            EventSystemManager.OnMasterBookClosed -= RecipeBookClosedFirstTime;
            EventSystemManager.OnIngredientPoured -= IngredientPoured;
            EventSystemManager.OnGarnishAdded -= GarnishAdded;
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
            
            recipeBookIcon.SetActive(false);
            recipeBookClosingIcon.interactable = false;
            mixButton.interactable = false;
            resetButton.interactable = false;
            trashButton.interactable = false;
            waterButton.interactable = false;
            serveButton.interactable = false;
        }
        
        private void LoadSteps()
        {
            // read Tutorial Steps json and create steps list
            string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/TutorialData/Tutorial1.json");
            
            //_steps = JsonConvert.DeserializeObject<CustomerList>(jsonString).customers;
            _steps = JsonConvert.DeserializeObject<List<List<string>>>(jsonString);
            
        }

        // this function will be used to proceed to the next step of the tutorial
        private void NextStep()
        {
            _actualStep++;
            try
            {
                _currentStep = _steps[_actualStep];
            }
            catch
            {
                _currentStep = null;
            }
            
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
                case 4: Step4();
                    break;
                case 5: Step5();
                    break;
                case 6: Step6();
                    break;
                case 7: Step7();
                    break;
                case 8: Step8();
                    break;
                case 9: Step9();
                    break;
                case 10: Step10();
                    break;
                case 11: Step11();
                    break;
                case 12: Step12();
                    break;
                case 13: Step13();
                    break;
                case 14: Step14();
                    break;
                case 15: Step15();
                    break;
                case 16: Step16();
                    break;
                case 17: Step17();
                    break;
                case 18: Step18();
                    break;
                case 19:
                    EndTutorial();
                    break;
                default:
                    break;
            }
        }

        // In Step0 Ernest will pop in with an introductory message
        private void Step0()
        {
            Debug.Log("Step 0");
            
            _currentImage.sprite = ernestSprite;
            customerCanvas.GetComponent<FadeCanvas>().FadeIn();
            StartCoroutine(WaitAndGreetDialogue());
            EventSystemManager.OnCustomerEnter();
        }

        // Ernest explains the CCA to the player
        private void Step1()
        {
            Debug.Log("Step 1");
            
            // CCA slides in 
            EventSystemManager.OnMakeCocktail(CocktailType.Wrong);
            
            // CCA becomes not interactable
            EventSystemManager.MakeIngredientInteractable(IngredientType.Unspecified);

            // Ernest pop up message
            StartCoroutine(WaitAndPopUp(false));
        }

        // Ernest returns to the dialogue view and tells the player to make their first cocktail
        private void Step2()
        {
            Debug.Log("Step 2");
            
            // CCA slides out
            EventSystemManager.HideCCA();
            
            // Ernest asks to make a Ripple
            StartCoroutine(WaitAndGreetDialogue());
        }

        // Shows the order post-it
        private void Step3()
        {
            Debug.Log("Step 3");
            
            // CCA slides in
            EventSystemManager.OnMakeCocktail(CocktailType.Ripple);
            
            // Ernest pop up message
            StartCoroutine(WaitAndPopUp(false));
            
            // Outline post-it
            StartCoroutine(FadeOutlineContinuous(postIt.transform.Find("Outline").gameObject));
        }
        
        // Ernest tells the player to open the recipe book
        private void Step4()
        {
            Debug.Log("Step 4");
            
            // Deactivate previous outline
            postIt.transform.Find("Outline").gameObject.SetActive(false);
            
            // Activate Recipe Book Button
            recipeBookIcon.SetActive(true);
            
            // Ernest pop up message
            StartCoroutine(WaitAndPopUp(true));
            
            // Outline recipe book button
            StartCoroutine(FadeOutlineContinuous(recipeBookIcon.transform.Find("Outline").gameObject));
        }

        // Ernest showing the recipe's ingredients to the player
        private void Step5()
        {
            Debug.Log("Step 5");
            
            // Show outline square
            StartCoroutine(FadeOutlineContinuous(ingredientSquare.gameObject));
            
            // Deactivate previous outline
            recipeBookIcon.transform.Find("Outline").gameObject.SetActive(false);
            
            // Ernest pop up message
            StartCoroutine(WaitAndPopUp(false));    
        }
        
        // Ernest asking the player to close the recipe book
        private void Step6()
        {
            Debug.Log("Step 6");
            
            // Hide outline square
            ingredientSquare.gameObject.SetActive(false);
            
            // Make X button interactable and outline it
            recipeBookClosingIcon.interactable = true;
            
            // Ernest pop up message
            StartCoroutine(WaitAndPopUp(true));
            
            // Outline X Button
            StartCoroutine(FadeOutlineContinuous(recipeBookClosingIcon.transform.Find("Outline").gameObject));
        }

        // Ernest asking to put the Caledon onto the shaker
        private void Step7()
        {
            Debug.Log("Step 7");
            
            // Deactivate previous outline
            recipeBookClosingIcon.transform.Find("Outline").gameObject.SetActive(false);
            
            // Caledon ingredient becomes interactable
            EventSystemManager.MakeIngredientInteractable(IngredientType.Caledon);
            
            // Ernest pop up message
            StartCoroutine(WaitAndPopUp(true));
            
            // Outline Caledon
            StartCoroutine(FadeOutlineContinuous(caledonImage.transform.Find("Outline").gameObject));
            
            // Outline shaker
            StartCoroutine(FadeOutlineContinuous(shakerOutline.gameObject));
        }
        
        // Ernest asking to put the Caledon onto the shaker again
        private void Step8()
        {
            Debug.Log("Step 8");
            
            // Ernest pop up message
            StartCoroutine(WaitAndPopUp(true));
        }
        
        // Ernest asking to put the Shaddock onto the shaker
        private void Step9()
        {
            Debug.Log("Step 9");
            
            // Deactivate Caledon outline
            caledonImage.transform.Find("Outline").gameObject.SetActive(false);
            
            // Shaddock ingredient becomes interactable
            EventSystemManager.MakeIngredientInteractable(IngredientType.Shaddock);
            
            // Ernest pop up message
            StartCoroutine(WaitAndPopUp(true));
            
            // Outline shaddock
            StartCoroutine(FadeOutlineContinuous(shaddockImage.transform.Find("Outline").gameObject));
        }
        
        // Ernest asking to put the Shaddock onto the shaker again
        private void Step10()
        {
            Debug.Log("Step 10");
            
            // Ernest pop up message
            StartCoroutine(WaitAndPopUp(true));
        }
        
        // Ernest asking to put the Gryte onto the shaker
        private void Step11()
        {
            Debug.Log("Step 11");
            
            // Deactivate shaddock outline
            shaddockImage.transform.Find("Outline").gameObject.SetActive(false);
            
            // Shaddock ingredient becomes interactable
            EventSystemManager.MakeIngredientInteractable(IngredientType.Gryte);
            
            // Ernest pop up message
            StartCoroutine(WaitAndPopUp(true));
            
            // Outline Gryte
            StartCoroutine(FadeOutlineContinuous(gryteImage.transform.Find("Outline").gameObject));
        }
        
        // Ernest asking to mix
        private void Step12()
        {
            Debug.Log("Step 12");
            
            // Deactivate Gryte outline and shaker outline
            gryteImage.transform.Find("Outline").gameObject.SetActive(false);
            shakerOutline.gameObject.SetActive(false);
            
            // Ingredient becomes not interactable
            EventSystemManager.MakeIngredientInteractable(IngredientType.Unspecified);
            
            // Stop all coroutines accumulated so far
            StopAllCoroutines();
            
            // Ernest pop up message
            StartCoroutine(WaitAndPopUp(true));
            
            // Outline mix button
            StartCoroutine(FadeOutlineContinuous(mixButton.transform.Find("Outline").gameObject));
        }

        // Ernest explains the trash button
        private void Step13()
        {
            Debug.Log("Step 13");
            
            // Make the water down button non interactable again (it gets activated for some reason)
            waterButton.interactable = false;
            
            // Deactivate mix button outline
            mixButton.transform.Find("Outline").gameObject.SetActive(false);
            
            // Ernest pop up message
            StartCoroutine(WaitAndPopUp(false));
            
            // Outline trash button
            StartCoroutine(FadeOutlineContinuous(trashButton.transform.Find("Outline").gameObject));
        }
        
        // Ernest explains the water down button
        private void Step14()
        {
            Debug.Log("Step 14");
            
            // Deactivate outline trash button
            trashButton.transform.Find("Outline").gameObject.SetActive(false);
            
            // Ernest pop up message
            StartCoroutine(WaitAndPopUp(false));
            
            // Outline water down button
            StartCoroutine(FadeOutlineContinuous(waterButton.transform.Find("Outline").gameObject));
        }
        
        // Ernest asking to serve
        private void Step15()
        {
            Debug.Log("Step 15");
            
            // Deactivate outline water button
            waterButton.transform.Find("Outline").gameObject.SetActive(false);
            
            // Make serve button interactable
            serveButton.interactable = true;
            
            // Ernest pop up message
            StartCoroutine(WaitAndPopUp(true));
            
            // Outline serve button
            StartCoroutine(FadeOutlineContinuous(serveButton.transform.Find("Outline").gameObject));
        }
        
        // Ernest asking to drag and drop the garnish
        private void Step16()
        {
            Debug.Log("Step 16");
            
            // Deactivate serve button
            serveButton.transform.Find("Outline").gameObject.SetActive(false);
            
            // Ernest pop up message
            StartCoroutine(WaitAndPopUp(true));
            
            // Outline garnish
            // todo
        }
        
        // Ernest congrats the player for its first cocktail
        private void Step17()
        {
            Debug.Log("Step 17");
            
            // Ernest dialogue
            StartCoroutine(WaitAndGreetDialogue());
        }
        
        // Name bar
        private void Step18()
        {
            Debug.Log("Step 18");
            
            // Ernest leaves
            customerCanvas.GetComponent<FadeCanvas>().FadeOut();
            customerCocktail.GetComponent<FadeCanvas>().FadeOut();
            
            // Pops up "name your bar" dialog with a small delay
            StartCoroutine(FadeNameBarCanvasDelayed());
        }

        private IEnumerator FadeNameBarCanvasDelayed()
        {
            yield return new WaitForSeconds(0.5f);
            nameYourBarCanvas.FadeIn();
            
            // Hacky fix for caret position...
            nameYourBarCanvas.transform.Find("InputField/Text Area/Text").GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.65f);
        }

        public void NameYourBar()
        {
            string name = nameYourBarCanvas.GetComponentInChildren<TMP_InputField>().text; // double-check that this is the right field to fetch
            GameData.BarName = name is null or "" ? "The Chitchat" : name;
            nameYourBarCanvas.FadeOut(); 
            
            NextStep();
        }
        
        private void EndTutorial()
        {
            Debug.Log("Tutorial ended");
            
            // Make all ingredients interactable again
            EventSystemManager.MakeAllIngredientsInteractable();
            
            // Make all buttons interactable again
            resetButton.interactable = true;
            trashButton.interactable = true;
            waterButton.interactable = true;
            
            // End the day
            EventSystemManager.OnTutorial1End();
            
        }
        
        private IEnumerator WaitAndGreetDialogue()
        {
            yield return new WaitForSeconds(timeBeforeDialogue);
            
            _dialogueManager.StartDialogue(
                new Dialogue("Ernest", _currentStep),
                DialogueType.Tutorial);
            
        }

        private IEnumerator WaitAndPopUp(bool isActionNeeded)
        {
            yield return new WaitForSeconds(timeBeforeDialogue);
            
            _dialogueManager.StartPopUp(new Dialogue("Ernest", _currentStep), isActionNeeded);
            
        }

        public void EndPopUp()
        {
            _dialogueManager.EndPopUp();
        }

        private void RecipeBookOpenedFirstTime()
        {
            if (_actualStep == 4)
            {
                EndPopUp();
            }
        }

        private void RecipeBookClosedFirstTime()
        {
            if (_actualStep == 6)
            {
                EndPopUp();
            }
        }

        // Note: the argument here is only needed in order to re-use an already existing event
        private void IngredientPoured(IngredientType ingredient)
        {
            if (_actualStep >= 7 && _actualStep <= 11)
            {
                EndPopUp();
            }
        }

        public void CocktailMixed()
        {
            if (_actualStep == 12)
            {
                EndPopUp();
            }
        }

        public void CocktailServed()
        {
            if (_actualStep == 15)
            {
                EndPopUp();
            }
        }

        private void GarnishAdded()
        {
            print("Garnish added");
            if (_actualStep == 16)
            {
                EndPopUp();
            }
        }

        private IEnumerator FadeOutlineContinuous(GameObject outlineObject)
        {
            outlineObject.SetActive(true);
            Image img = outlineObject.GetComponentInChildren<Image>();
            
            float fadeDuration = 0.75f;
            while(true)
            {
                yield return StartCoroutine(FadeAlpha(img, 0f, 1f, fadeDuration));
                yield return StartCoroutine(FadeAlpha(img, 1f, 0f, fadeDuration));
            }
        }

        private IEnumerator FadeAlpha(Image img, float startAlpha, float endAlpha, float duration)
        {
            float elapsed = 0f;
            Color c = img.color;
            while(elapsed < duration)
            {
                float t = elapsed / duration;
                c.a = Mathf.Lerp(startAlpha, endAlpha, t);
                img.color = c;
                elapsed += Time.deltaTime;
                yield return null;
            }
            c.a = endAlpha;
            img.color = c;
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
