using Bar;
using UnityEngine;

namespace Technical
{
    public class ClickDialogueBox : MonoBehaviour
    {
        [SerializeField] private DialogueManager dialogueManager;
    
        public void OnClick()
        {
            dialogueManager.OnNextButtonPressed();
        }
    }
}
