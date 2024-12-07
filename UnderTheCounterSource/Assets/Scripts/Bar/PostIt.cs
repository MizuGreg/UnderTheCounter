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
            EventSystemManager.OnOverwritePostIt += OverwritePostIt;
        }

        private void OnDestroy()
        {
            EventSystemManager.OnOverwritePostIt -= OverwritePostIt;
        }

        private void OverwritePostIt(string _actualOrder)
        {
            actualOrder = _actualOrder;
            print("PostIt overwriting");
        }

        public void WriteCocktail(CocktailType cocktailType) {
            if (cocktailType != CocktailType.Wrong) {
                if (actualOrder == "")
                {
                    cocktailName.text = cocktailType.ToString();
                    // edge case here... should be handled better and not hardcoded
                    if (cocktailName.text == "SpringBee") cocktailName.text = "Spring Bee";
                }
                else
                {
                    print("im here");
                    cocktailName.text = actualOrder;
                    actualOrder = "";
                }

                animator.SetBool(IsPostItShown, true);
            }
        }

        public void HidePostIt() {
            animator.SetBool(IsPostItShown, false);
        }
        
    }
}