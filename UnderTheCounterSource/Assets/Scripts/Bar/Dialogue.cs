using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;

    [TextArea(3, 10)]
    public Queue<string> sentences;

    public Dialogue(string name, Queue<string> sentences)
    {
        this.name = name;
        this.sentences = sentences;
    }
}