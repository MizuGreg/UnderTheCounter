using System.Collections.Generic;
using UnityEngine;

namespace Bar
{
    public class DialogueManager : MonoBehaviour
    {
        public CanvasGroup dialogueCanvas;
    
        public void customerGreet(Queue<string> lines)
        {
            // todo: customer greets bartender
            // button, spacebar, or clicking will call customerRequestOrder
            // (hint: create a helper function for detecting these three inputs since it's recurring)
            // in the future the text will appear gradually, and clicking once just shows the whole text.
            // clicking twice does as above
            print(lines.Peek());
        }

        public void customerRequestOrder(Queue<string> lines)
        {
            // todo: this displays a second, shorter line that prompts the order when finished
            // button, spacebar or clicking will close dialogue and throw event onPreparationStart
            print(lines.Peek());
        }

        public void customerServe(Queue<string> lines)
        {
            // todo: displays a final line where the customer comments on the cocktail served
            // button, spacebar or clicking will close dialogue, wait just a little bit, and throw event onCustomerLeave
            print(lines.Peek());
        }
    
    }
}
