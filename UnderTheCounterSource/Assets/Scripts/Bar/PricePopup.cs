using TMPro;
using UnityEngine;

namespace Bar
{
    public class PricePopup : MonoBehaviour
    {
        private static readonly int IsPopping = Animator.StringToHash("isPopping");

        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private Animator animator;
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void DisplayPrice(float earning)
        {
            priceText.text = $"${earning:F}";
            animator.SetTrigger(IsPopping);
        }
    }
}
