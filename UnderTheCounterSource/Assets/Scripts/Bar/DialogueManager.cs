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

        private Queue<string> _sentences;
        private DialogueType _dialogueType;
        private Coroutine _typeSentenceCoroutine;
        private bool _isBoxActive;

        private float textSpeed;
        [Range(1f, 50.0f)]
        public float normalTextSpeed;
        [Range(50f, 500.0f)]
        public float skipTextSpeed;
        [Range(5f, 50.0f)]
        public float punctuationWaitMultiplier;
        
        [Range(0.1f, 3.0f)]
        public float timeBeforeDialogueBox;
        [Range(0.1f, 3.0f)]
        public float timeBeforeFirstSentence;

        private bool _allTextIsVisible;
        
        // Initialization
        void Start() {
            dialogueText.transform.parent.gameObject.SetActive(true);
            _sentences = new Queue<string>();
            textInfo = dialogueText.textInfo;
        }

        public void SetNormalTextSpeed(float speed)
        {
            normalTextSpeed = speed;
            textSpeed = speed; // so this also stops the skipping animation, as a side effect
        }

        public void StartDialogue(Dialogue dialogue, DialogueType dialogueType)
        {
            StartCoroutine(WaitBeforeDialogueBox(dialogue, dialogueType));
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
            _isBoxActive = true;
            DisplayNextSentence();
        }

        public void OnNextButtonPressed(InputAction.CallbackContext context)
        {
            if (_isBoxActive & context.performed)
            {
                if (_allTextIsVisible) DisplayNextSentence();
                else SkipText();
            }
        }

        private void DisplayNextSentence()
        {
            textSpeed = normalTextSpeed;
            if (_sentences.Count == 0) {
                _isBoxActive = false;
                EndDialogue();
                switch (_dialogueType)
                {
                    case DialogueType.Greet:
                        EventSystemManager.OnMakeCocktail();
                        break;
                    case DialogueType.Leave:
                        EventSystemManager.OnCustomerLeave();
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
            textSpeed = skipTextSpeed;
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