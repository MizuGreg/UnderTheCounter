using CocktailCreation;
using TMPro;
using UnityEngine;

namespace Bar
{
    public class PostIt : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI cocktailName;
        [SerializeField] private Animator animator;
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void WriteCocktail(CocktailType cocktailType) {
            if (cocktailType != CocktailType.Wrong) {
                cocktailName.text = cocktailType.ToString();
                // edge case here... should be handled better and not hardcoded
                if (cocktailName.text == "SpringBee") cocktailName.text = "Spring Bee";
                animator.SetBool(IsOpen, true);
            }
        }

        public void HidePostIt() {
            animator.SetBool(IsOpen, false);
        }
        
    }
}