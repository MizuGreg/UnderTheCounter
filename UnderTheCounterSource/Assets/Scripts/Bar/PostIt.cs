using CocktailCreation;
using Technical;
using TMPro;
using UnityEngine;

namespace Bar
{
    public class PostIt : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI cocktailName;
        [SerializeField] private Animator animator;
        private static readonly int IsPostItShown = Animator.StringToHash("IsPostItShown");

        private string actualOrder = "";

        private void Awake()
        {
            animator = GetComponent<Animator>();
            EventSystemManager.OnWritePostIt += WritePostIt;
        }

        private void OnDestroy()
        {
            EventSystemManager.OnWritePostIt -= WritePostIt;
        }

        private void WritePostIt(string order)
        {
            cocktailName.text = order;
        }

        public void ShowPostIt()
        {
            animator.SetBool(IsPostItShown, true);
        }

        public void HidePostIt() {
            animator.SetBool(IsPostItShown, false);
        }
        
    }
}