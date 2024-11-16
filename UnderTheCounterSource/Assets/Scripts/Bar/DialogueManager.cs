using System.Collections;
using System.Collections.Generic;
using Bar;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour {
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Animator animator;

    private Queue<string> _sentences;
    private DialogueType _dialogueType;
    
    
    [Range(1f, 50.0f)]
    public float textSpeed;
    [Range(0.5f, 2.0f)]
    public float timeBeforeDialogueBox;
    [Range(0.5f, 2.0f)]
    public float timeBeforeFirstSentence;

    private bool _allTextIsVisible;

    // Initialization
    void Start() {
        _sentences = new Queue<string>();
    }

    public IEnumerator StartDialogue(Dialogue dialogue, DialogueType dialogueType) 
    {
        yield return new WaitForSeconds(timeBeforeDialogueBox);
        animator.SetBool(IsOpen, true);
        nameText.text = dialogue.name;
        _dialogueType = dialogueType;

        _sentences.Clear();

        foreach (string sentence in dialogue.sentences) 
        {
            _sentences.Enqueue(sentence);
        }

        dialogueText.text = "";
        yield return new WaitForSeconds(timeBeforeFirstSentence);
        DisplayNextSentence();
    }

    private IEnumerator DisplayFirstSentence()
    {
        dialogueText.text = "";
        yield return new WaitForSeconds(timeBeforeFirstSentence);
        DisplayNextSentence();
    }

    public void OnNextButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (_allTextIsVisible) DisplayNextSentence();
            else DisplayAllText();
        }
    }

    private void DisplayNextSentence() 
    {

        if (_sentences.Count == 0) {
            EndDialogue();
            EventSystemManager.OnMakeCocktail();
            return;
        }

        if (_sentences.Count == 1)
        {
            // todo: change arrow into customized icon based on dialogue type
        }

        string sentence = _sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
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