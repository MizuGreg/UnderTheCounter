using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MC_Dialogue
{
    public string name;

    [TextArea(3, 10)]
    public string[] sentences;

    public bool choices;
    public string[] choicesText;
}