using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bar
{
    [Serializable]
    public class Dialogue
    {
        public string name;

        [TextArea(3, 10)]
        public List<string> sentences;

        public Dialogue(string name, List<string> sentences)
        {
            this.name = name;
            this.sentences = sentences;
        }
        
    }
    
    [Serializable]
    public enum DialogueType
    {
        Greet,
        Leave,
        Tutorial,
        NoDrink,
        Blitz
    }
}