using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SavedGameData;
using Technical;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Bar
{
    public class DialogueManager : MonoBehaviour {
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        [Header("Dialogue objects")]
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI dialogueText;
        private TMP_TextInfo textInfo;
        [SerializeField] private Animator animator;
        
        [SerializeField] private Image arrow;
        private Sprite normalArrow, makeCocktailArrow, leaveArrow;
        
        [Header("Pop-up objects")]
        [SerializeField] private TextMeshProUGUI popUpNameText;
        [SerializeField] private TextMeshProUGUI popUpDialogueText;
        [SerializeField] private Image popUpArrow;
        private bool isPopupActive;
        private bool _isActionNeeded;

        private Queue<string> _sentences = new();
        private DialogueType _dialogueType;
        private Coroutine _typeSentenceCoroutine;
        private bool isBoxActive;

        [Header("Timing-related variables")]
        private float textSpeed;
        [Range(1f, 50.0f)]
        [SerializeField] private float normalTextSpeed;

        [Range(5f, 50.0f)]
        [SerializeField] private float punctuationWaitMultiplier;
        
        [Range(0.1f, 3.0f)]
        [SerializeField] private float timeBeforeDialogueBox;
        [Range(0.1f, 3.0f)]
        [SerializeField] private float timeBeforeFirstSentence;

        private bool _allTextIsVisible;
        
        // Initialization
        void Start() {
            if (dialogueText != null) // we initialize correctly only if we're in the proper scene
            {
                dialogueText.transform.parent.gameObject.SetActive(true);
                textInfo = dialogueText.textInfo;
            }
            
            normalArrow = Resources.Load<Sprite>("Sprites/(Old)/UI/arrow");
            makeCocktailArrow = Resources.Load<Sprite>("Sprites/(Old)/UI/glass");
            leaveArrow = Resources.Load<Sprite>("Sprites/(Old)/UI/bottle_opener");
        }

        public void SetNormalTextSpeed(float speed)
        {
            normalTextSpeed = speed;
            textSpeed = speed;
        }

        public void StartPopUp(Dialogue dialogue, bool isActionNeeded)
        {
            isPopupActive = true;
            popUpNameText.text = dialogue.name;
            _isActionNeeded = isActionNeeded;
            
            _sentences.Clear();
            foreach (string sentence in dialogue.sentences) 
            {
                _sentences.Enqueue(sentence);
            }
            
            popUpDialogueText.text = "";
            popUpDialogueText.transform.parent.GetComponent<FadeCanvas>().FadeIn();
            popUpArrow.gameObject.SetActive(!isActionNeeded);
            ShowNextPopUp();
        }

        public void ShowNextPopUp()
        {
            if (_sentences.Count > 0)
            {
                popUpDialogueText.text = _sentences.Dequeue();
            }
            else if (_sentences.Count == 0)
            {
                StopAllCoroutines();

                if (!_isActionNeeded)
                {
                    EndPopUp();
                }
            }
        }

        public void EndPopUp()
        {
            isPopupActive = false;
            popUpDialogueText.transform.parent.GetComponent<FadeCanvas>().FadeOut();
            _isActionNeeded = false;
            EventSystemManager.NextTutorialStep();
        }
        
        public void StartDialogue(Dialogue dialogue, DialogueType dialogueType)
        {
            _sentences.Clear();
            StartCoroutine(WaitBeforeDialogueBox(dialogue, dialogueType));
        }

        private void EndDialogue()
        {
            animator.SetBool(IsOpen, false);
        }

        private IEnumerator WaitBeforeDialogueBox(Dialogue dialogue, DialogueType dialogueType)
        {
            yield return new WaitForSeconds(timeBeforeDialogueBox);
            animator.SetBool(IsOpen, true);
            nameText.text = dialogue.name;
            _dialogueType = dialogueType;
            arrow.GetComponent<Image>().sprite = normalArrow;
            
            foreach (string sentence in dialogue.sentences) 
            {
                ParseLineTag(sentence);
            }
            dialogueText.text = "";
            StartCoroutine(WaitBeforeFirstSentence());
        }

        private IEnumerator WaitBeforeFirstSentence()
        {
            yield return new WaitForSeconds(timeBeforeFirstSentence);
            isBoxActive = true;
            DisplayNextSentence();
        }

        public void OnNextButtonPressed(InputAction.CallbackContext context)
        {
            if (context.performed) // works only when spacebar is lifted
                OnNextButtonPressed();
        }

        public void OnNextButtonPressed()
        {
            if (isPopupActive)
            {
                ShowNextPopUp();
                return;
            }
            
            if (isBoxActive) // so this works only when the dialogue box is fully displayed and running
            {
                if (_allTextIsVisible) DisplayNextSentence();
                else SkipText();
            }
        }
        
        private void SkipText()
        {
            _allTextIsVisible = true;
            dialogueText.maxVisibleCharacters = dialogueText.text.Length;
            arrow.gameObject.SetActive(true);
        }

        private void DisplayNextSentence()
        {
            arrow.gameObject.SetActive(false);
            textSpeed = normalTextSpeed;
            if (_sentences.Count == 0) {
                if (_dialogueType != DialogueType.MultipleChoice) {
                    isBoxActive = false;
                    EndDialogue();
                }
                switch (_dialogueType)
                {
                    case DialogueType.Greet:
                        EventSystemManager.OnPreparationStart();
                        break;
                    case DialogueType.Leave:
                        EventSystemManager.OnCustomerLeave();
                        break;
                    case DialogueType.Tutorial:
                        EventSystemManager.NextTutorialStep();
                        break;
                    case DialogueType.NoDrink:
                        EventSystemManager.OnCustomerLeave();
                        break;
                    case DialogueType.MultipleChoice:
                        EventSystemManager.MultipleChoiceStart();
                        isBoxActive = false; // deactivates interaction with box, e.g. clicking to skip sentence
                        break;
                }
            }
            else
            {
                string sentence = _sentences.Dequeue();
                sentence = ParseEventTag(sentence);
                GameData.Log.Add(new Tuple<string, string>(nameText.text, sentence));
                
                StartCoroutine(TypeSentence(sentence));
            }
        }
        
        private void ParseLineTag(string sentence)
        {
            if (sentence.Contains("["))
            {
                string tag = sentence.Substring(sentence.IndexOf("[") + 1, sentence.IndexOf("]") - sentence.IndexOf("[") - 1); // extracts text inside brackets
                string strippedSentence = sentence.Remove(sentence.IndexOf("["), sentence.IndexOf("]") - sentence.IndexOf("[") + 1); // removes whole tag with brackets
                switch (tag)
                {
                    case "IF HAS POSTERS":
                        if (GameData.Posters.Count > 0) _sentences.Enqueue(strippedSentence);
                        break;
                    case "IF DOESN'T HAVE POSTERS":
                        if (GameData.Posters.Count == 0) _sentences.Enqueue(strippedSentence);
                        break;
                    case "IF MARGARET GOT DRUNK":
                        if (GameData.Choices["MargaretDrunk"]) _sentences.Enqueue(strippedSentence);
                        break;
                    case "IF MARGARET DIDN'T GET DRUNK":
                        if (!GameData.Choices["MargaretDrunk"]) _sentences.Enqueue(strippedSentence);
                        break;
                    case "IF MAFIA DEAL ACCEPTED":
                        if (GameData.Choices["MafiaDeal"]) _sentences.Enqueue(strippedSentence);
                        break;
                    case "IF MAFIA DEAL NOT ACCEPTED":
                        if (!GameData.Choices["MafiaDeal"]) _sentences.Enqueue(strippedSentence);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                _sentences.Enqueue(sentence);
            }
        }

        private string ParseEventTag(string sentence)
        {
            if (sentence.Contains("{"))
            {
                string tag = sentence.Substring(sentence.IndexOf("{") + 1, sentence.IndexOf("}") - sentence.IndexOf("{") - 1); // extracts text inside brackets
                int tagIndex = sentence.IndexOf("{");
                sentence = sentence.Remove(sentence.IndexOf("{"), sentence.IndexOf("}") - sentence.IndexOf("{") + 1); // removes whole tag with brackets
                
                if (tag.Contains("TRINKET"))
                {
                    int trinketID = int.Parse(Regex.Match(tag, @"\d+").Value); // extracts ID
                    EventSystemManager.OnTrinketObtained(trinketID);
                }
                if (tag.Contains("POSTER"))
                {
                    int posterID = int.Parse(Regex.Match(tag, @"\d+").Value);
                    EventSystemManager.OnPosterObtained(posterID);
                }
                if (tag == "MARGARET GETS DRUNK")
                {
                    GameData.Choices["MargaretDrunk"] = true;
                }
                if (tag == "MAFIA DEAL ACCEPTED")
                {
                    GameData.Choices["MafiaDeal"] = true;
                }

                if (tag == "PIZZO PAID")
                {
                    GameData.Choices["PizzoPaid"] = true;
                }
                if (tag == "UNDERCOVER CATCHES YOU")
                { // makes next blitz extremely likely
                    GameData.BlitzFailed(); // penalizes maxDrunkCustomers and blitz time
                    if (GameData.DrunkCustomers <= GameData.MaxDrunkCustomers - 1)
                        GameData.DrunkCustomers = GameData.MaxDrunkCustomers - 1; // next alcoholic beverage triggers blitz
                }
                if (tag == "BAR NAME")
                {
                    sentence = sentence.Insert(tagIndex, GameData.BarName);
                }
            }

            return sentence;
        }

        private IEnumerator TypeSentence(string sentence) 
        {
            _allTextIsVisible = false;
            dialogueText.text = sentence; // sentence is fed into box
            dialogueText.ForceMeshUpdate(); // force correct calculation of characters
            
            dialogueText.maxVisibleCharacters = 0;
            int currentVisibleCharIndex = 0;

            while (!_allTextIsVisible)
            {
                if (currentVisibleCharIndex >= textInfo.characterCount - 1) // means we're displaying the last character
                {
                    dialogueText.maxVisibleCharacters++;
                    _allTextIsVisible = true;
                    break;
                }
                // otherwise, get current character
                var character = textInfo.characterInfo[currentVisibleCharIndex].character;
                dialogueText.maxVisibleCharacters++; // shows character
                if (character is ',' or ';')
                {
                    yield return new WaitForSeconds(punctuationWaitMultiplier / textSpeed);
                }
                else if (character is '.' or '!' or '?' or '…')
                {
                    yield return new WaitForSeconds(punctuationWaitMultiplier*3 / textSpeed);
                }
                else
                {
                    yield return new WaitForSeconds(1 / textSpeed);
                }
                currentVisibleCharIndex++;
            }
            arrow.gameObject.SetActive(true);
        }
    }
}