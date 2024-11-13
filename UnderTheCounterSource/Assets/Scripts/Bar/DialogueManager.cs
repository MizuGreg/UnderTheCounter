using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour {
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Animator animator;

    private Queue<string> sentences;
    [Range(1f, 50.0f)]
    public float textSpeed;

    [Range(0.5f, 2.0f)]
    public float timeBeforeFirstSentence;

    private bool _allTextIsVisible;

    // Initialization
    void Start() {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue) 
    {
        animator.SetBool(IsOpen, true);
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences) 
        {
            sentences.Enqueue(sentence);
        }

        StartCoroutine(DisplayFirstSentence());
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

        if (sentences.Count == 0) {
            EndDialogue();
            EventSystemManager.OnMakeCocktail();
            return;
        }

        string sentence = sentences.Dequeue();
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
        if (sentences.Count == 0)
        {
            // show either cocktail or exit sign?
        }
        else
        {
            // show normal arrow
        }
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