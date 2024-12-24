using System.Collections;
using System.Collections.Generic;   
using UnityEngine;
using UnityEngine.UI;

public class MC_DialogueManager : MonoBehaviour {

    public Text nameText;
    public Text dialogueText;
    public Animator animator;

    public Button choiceButton1;
    public Button choiceButton2;

    private Queue<string> sentences;
    private bool choice;
    private string choiceSentence1;
    private string choiceSentence2;

    // Initialization
    void Start() {
        sentences = new Queue<string>();
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
    }

    public void StartDialogue(MC_Dialogue mcDialogue) 
    {
        animator.SetBool("IsOpen", true);
        animator.SetBool("Choice", false);
        nameText.text = mcDialogue.name;

        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);

        sentences.Clear();

        foreach (string sentence in mcDialogue.sentences) 
        {
            sentences.Enqueue(sentence);
        }
        choice = mcDialogue.choices;

        if (choice)
        {
            choiceSentence1 = mcDialogue.choicesText[0];
            choiceSentence2 = mcDialogue.choicesText[1];
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() 
    {

        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence) 
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()) 
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue() 
    {
        if (!choice) 
        {
            animator.SetBool("IsOpen", false);
        }
        else 
        {
            ShowChoiceButtons();
        }
    }

    void ShowChoiceButtons() 
    {
        choiceButton1.gameObject.SetActive(true);
        choiceButton2.gameObject.SetActive(true);

        choiceButton1.GetComponentInChildren<Text>().text = choiceSentence1;
        choiceButton2.GetComponentInChildren<Text>().text = choiceSentence2;

        choiceButton1.onClick.AddListener(() => OnChoiceSelected(0));
        choiceButton2.onClick.AddListener(() => OnChoiceSelected(1));
    }

    void OnChoiceSelected(int choiceIndex)
    {
        // Logica per la scelta effettuata (da usare come stub per la chiamata al manager)
        Debug.Log("Choice selected: " + choiceIndex);

        // Nascondi i pulsanti dopo aver fatto la scelta
        choiceButton1.gameObject.SetActive(false);
        choiceButton2.gameObject.SetActive(false);
    }

}