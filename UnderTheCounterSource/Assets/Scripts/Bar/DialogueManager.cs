using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    public void DisplayNextSentence() 
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

    private IEnumerator TypeSentence(string sentence) 
    {
        dialogueText.text = sentence;
        dialogueText.maxVisibleCharacters = 0;
        foreach (char letter in sentence) 
        {
            dialogueText.maxVisibleCharacters++;
            yield return new WaitForSeconds(1 / (textSpeed));
        }
    }

    private void EndDialogue() 
    {
        animator.SetBool(IsOpen, false);
    }

}