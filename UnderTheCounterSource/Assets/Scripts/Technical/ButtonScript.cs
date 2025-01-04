using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Technical
{
    public class ButtonScript : MonoBehaviour
    {
        private Button button;
        private TextMeshProUGUI text;
        private CanvasGroup triangles;
        private bool active;

        private void Awake()
        {
            button = GetComponent<Button>();
            text = GetComponentInChildren<TextMeshProUGUI>();
            triangles = transform.Find("Triangles").GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            triangles.alpha = 0;
            active = button.interactable;
            UpdateAlpha();
        }

        public void Update()
        {
            if (button.interactable != active)
            {
                active = button.interactable;
                UpdateAlpha();
            }
        }

        private void UpdateAlpha()
        {
            if (active)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
            }
            else
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0.5f);
                triangles.alpha = 0;
            }
        }
    
        public void PointerEnter()
        {
            if (active)
            {
                triangles.alpha = 1;
            }
        }

        public void PointerExit()
        {
            if (active)
            {
                triangles.alpha = 0;
            }
        }
    }
}
