using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PosterMenuButtonController : MonoBehaviour
{
    private static readonly int IsOpen = Animator.StringToHash("isOpen");
    public Button openButton; // Reference to the button that will open the menu
    public GameObject overlayPanel; // Reference to the overlay menu panel
    public Animator panelAnimator; // Reference to the Animator on the panel
    private bool _isMenuOpen = false;

    private Vector2[] posterPositions;  // To store the last known positions of the posters
    private RectTransform[] posterTransforms;  // Store RectTransforms of the posters

    private void Start()
    {
        Debug.Log("Start called");
        // Initially set the panel's animation state to hidden
        panelAnimator.SetBool("IsOpen", _isMenuOpen);

        // Add a listener to the button
        openButton.onClick.AddListener(ToggleMenu);
    }

    private void ToggleMenu()
    {
        _isMenuOpen = !_isMenuOpen;
        Debug.Log("Button Clicked In New Way");

        if (_isMenuOpen)
        {
            Debug.Log("Open Menu");
            // Save the positions of all the posters before opening the menu
            panelAnimator.SetBool("IsOpen", true);  // Play opening animation
        }
        else
        {
            Debug.Log("Close Menu");
            panelAnimator.SetBool("IsOpen", false); // Play closing animation
        }
    }
}
