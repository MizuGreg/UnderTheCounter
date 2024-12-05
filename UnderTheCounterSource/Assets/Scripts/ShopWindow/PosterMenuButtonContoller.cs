using UnityEngine;
using UnityEngine.UI;

namespace ShopWindow
{
    public class PosterMenuButtonController : MonoBehaviour
    {
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");
        public Button openButton; // Reference to the button that will open the menu
        public GameObject overlayPanel; // Reference to the overlay menu panel
        public Animator panelAnimator; // Reference to the Animator on the panel
        private bool _isMenuOpen = false;

        private Vector2[] _posterPositions;  // To store the last known positions of the posters
        private RectTransform[] _posterTransforms;  // Store RectTransforms of the posters

        private void Start()
        {
            // Initially set the panel's animation state to hidden
            panelAnimator.SetBool(IsOpen, _isMenuOpen);

            // Add a listener to the button
            openButton.onClick.AddListener(ToggleMenu);
        }

        private void ToggleMenu()
        {
            _isMenuOpen = !_isMenuOpen;

            if (_isMenuOpen)
            {
                // Save the positions of all the posters before opening the menu
                panelAnimator.SetBool(IsOpen, true);  // Play opening animation
            }
            else
            {
                panelAnimator.SetBool(IsOpen, false); // Play closing animation
            }
        }
    }
}
