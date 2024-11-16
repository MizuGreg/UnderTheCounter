using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bar
{
    public class DialogueManager : MonoBehaviour {
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        public TextMeshProUGUI nameText;
        public TextMeshProUGUI dialogueText;
        public Animator animator;

        private Queue<string> _sentences;
        private DialogueType _dialogueType;

        private bool _isBoxActive;
    
        [Range(1f, 50.0f)]
        public float textSpeed;
        [Range(0.5f, 2.0f)]
        public float timeBeforeDialogueBox;
        [Range(0.5f, 2.0f)]
        public float timeBeforeFirstSentence;

        private bool _allTextIsVisible;

        // Initialization
        void Start() {
            dialogueText.transform.parent.gameObject.SetActive(true);
            _sentences = new Queue<string>();
        }

        public void StartDialogue(Dialogue dialogue, DialogueType dialogueType)
        {
            StartCoroutine(Wait(timeBeforeDialogueBox));
            animator.SetBool(IsOpen, true);
            nameText.text = dialogue.name;
            _dialogueType = dialogueType;

            _sentences.Clear();

            foreach (string sentence in dialogue.sentences) 
            {
                _sentences.Enqueue(sentence);
            }

            dialogueText.text = "";
            StartCoroutine(Wait(timeBeforeFirstSentence));
            _isBoxActive = true;
            DisplayNextSentence();
        }

        private IEnumerator Wait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
        }

        public void OnNextButtonPressed(InputAction.CallbackContext context)
        {
            if (_isBoxActive & context.performed)
            {
                if (_allTextIsVisible) DisplayNextSentence();
                else DisplayAllText();
            }
        }

        private void DisplayNextSentence() 
        {
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
                StopAllCoroutines();
                StartCoroutine(TypeSentence(sentence));
            }
        }

        private void DisplayAllText()
        {
            dialogueText.maxVisibleCharacters = dialogueText.text.Length;
            _allTextIsVisible = true;
        }

        private void showIcon()
        {
            // todo: make icon visible
        }

        private void hideIcon()
        {
            // todo: make icon invisible
        }

        private void updateIcon()
        {
            // todo: update icon based on dialogue type
        }

        private IEnumerator TypeSentence(string sentence) 
        {
            dialogueText.text = sentence;
            dialogueText.maxVisibleCharacters = 0;
            _allTextIsVisible = false;

            while (!_allTextIsVisible)
            {
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(1 / (textSpeed));
                if (dialogueText.maxVisibleCharacters >= dialogueText.text.Length) _allTextIsVisible = true;
            }
        }

        private void EndDialogue()
        {
            animator.SetBool(IsOpen, false);
        }

    }
}