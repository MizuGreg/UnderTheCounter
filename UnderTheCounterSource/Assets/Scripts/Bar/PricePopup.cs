using TMPro;
using UnityEngine;

namespace Bar
{
    public class PricePopup : MonoBehaviour
    {
        private static readonly int IsPopping = Animator.StringToHash("isPopping");

        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Animator animator;

        private void Start() {
            gameObject.SetActive(true);
        }

        public void DisplayPrice(float earning)
        {
            priceText.text = $"${earning:N0}";
            animator.SetTrigger(IsPopping);
        }
    }
}
