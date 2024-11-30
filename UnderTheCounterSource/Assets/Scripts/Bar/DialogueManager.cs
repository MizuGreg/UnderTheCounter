using System;
using System.Collections;
using System.Collections.Generic;
using Technical;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bar
{
    public class DialogueManager : MonoBehaviour {
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        public TextMeshProUGUI nameText;
        public TextMeshProUGUI dialogueText;
        private TMP_TextInfo textInfo;
        public Animator animator;
        
        // Pop-up messages
        public TextMeshProUGUI popUpNameText;
        public TextMeshProUGUI popUpDialogueText;

        private Queue<string> _sentences;
        private DialogueType _dialogueType;
        private Coroutine _typeSentenceCoroutine;
        private bool IsBoxActive;

        private float textSpeed;
        [Range(1f, 50.0f)]
        public float normalTextSpeed;

        [Range(5f, 50.0f)]
        public float punctuationWaitMultiplier;
        
        [Range(0.1f, 3.0f)]
        public float timeBeforeDialogueBox;
        [Range(0.1f, 3.0f)]
        public float timeBeforeFirstSentence;

        private bool _allTextIsVisible;
        
        // Initialization
        void Start() {
            if (dialogueText != null) // we initialize correctly only if we're in the proper scene
            {
                dialogueText.transform.parent.gameObject.SetActive(true);
                _sentences = new Queue<string>();
                textInfo = dialogueText.textInfo;
            }
        }

        public void setDialogueBoxActive(bool active)
        {
            IsBoxActive = active;
        }

        public void SetNormalTextSpeed(float speed)
        {
            normalTextSpeed = speed;
            textSpeed = speed; // so this also stops the skipping animation, as a side effect
        }

        public void StartDialogue(Dialogue dialogue, DialogueType dialogueType)
        {
            //DANGER
            _sentences.Clear();
            //DANGER
            
            
            StartCoroutine(WaitBeforeDialogueBox(dialogue, dialogueType));
        }

        public void StartPopUp(Dialogue dialogue)
        {
            popUpNameText.text = dialogue.name;
            foreach (string sentence in dialogue.sentences) 
            {
                _sentences.Enqueue(sentence);
            }
            popUpDialogueText.text = "";
            popUpDialogueText.text = dialogue.sentences[0]; // sentence is fed into box
            popUpDialogueText.ForceMeshUpdate(); // force correct calculation of characters
            
            popUpDialogueText.transform.parent.gameObject.SetActive(true);
            
            StartCoroutine(EndPopUpAfterAWhile());
        }

        private IEnumerator EndPopUpAfterAWhile()
        {
            yield return new WaitForSeconds(5);
            popUpDialogueText.transform.parent.gameObject.SetActive(false);
            EventSystemManager.NextTutorialStep();
        }

        private IEnumerator WaitBeforeDialogueBox(Dialogue dialogue, DialogueType dialogueType)
        {
            yield return new WaitForSeconds(timeBeforeDialogueBox);
            animator.SetBool(IsOpen, true);
            nameText.text = dialogue.name;
            _dialogueType = dialogueType;
            
            foreach (string sentence in dialogue.sentences) 
            {
                _sentences.Enqueue(sentence);
            }
            dialogueText.text = "";
            StartCoroutine(WaitBeforeFirstSentence());
        }

        private IEnumerator WaitBeforeFirstSentence()
        {
            yield return new WaitForSeconds(timeBeforeFirstSentence);
            IsBoxActive = true;
            DisplayNextSentence();
        }

        public void OnNextButtonPressed(InputAction.CallbackContext context)
        {
            if (context.performed) // works only when spacebar is lifted
                OnNextButtonPressed();
        }

        public void OnNextButtonPressed()
        {
            if (!IsBoxActive) return; // works only when the dialogue box is fully displayed and running
            if (_allTextIsVisible) DisplayNextSentence();
            else SkipText();
        }

        private void DisplayNextSentence()
        {
            textSpeed = normalTextSpeed;
            if (_sentences.Count == 0) {
                IsBoxActive = false;
                EndDialogue();
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
                }
            }
            else
            {
                if (_sentences.Count == 1)
                {
                    // todo: change arrow into customized icon based on dialogue type
                }
                string sentence = _sentences.Dequeue();
                if (_typeSentenceCoroutine != null) StopCoroutine(_typeSentenceCoroutine);
                StartCoroutine(TypeSentence(sentence));
            }
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
                    yield break;
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
        }
        
        private void SkipText()
        {
            _allTextIsVisible = true;
            dialogueText.maxVisibleCharacters = dialogueText.text.Length;
        }

        private void showIcon()
        {
            // todo: make icon visible
        }

        private void hideIcon()
        {
            // todo: hide icon
        }

        private void updateIcon()
        {
            // todo: update icon based on dialogue type
        }

        private void EndDialogue()
        {
            animator.SetBool(IsOpen, false);
        }

    }
}