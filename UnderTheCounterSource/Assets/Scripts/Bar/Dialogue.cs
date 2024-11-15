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
        public Queue<string> sentences;

        public Dialogue(string name, Queue<string> sentences)
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
        Inspector
    }
}