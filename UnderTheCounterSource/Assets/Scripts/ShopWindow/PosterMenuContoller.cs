using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PosterMenuController : MonoBehaviour
{
    private static readonly int IsOpen = Animator.StringToHash("isOpen");
    public Button openButton; // Reference to the button that will open the menu
    public GameObject overlayPanel; // Reference to the overlay menu panel
    public Animator panelAnimator; // Reference to the Animator on the panel
    private bool _isMenuOpen = false;

    private void Start()
    {
        // Ensure the menu is initially hidden
        // overlayPanel.SetActive(false);

        // Initially set the panel's animation state to hidden
        panelAnimator.SetBool(name:"IsOpen", _isMenuOpen);
        
        // Add a listener to the button
        openButton.onClick.AddListener(ToggleMenu);
    }

    private void ToggleMenu()
    {
        Debug.Log("button clicked");
        _isMenuOpen = !_isMenuOpen;
        overlayPanel.SetActive(true); // Make sure panel is visible when starting animation
        panelAnimator.SetBool("IsOpen", _isMenuOpen);

        // If closing the menu, disable panel after animation duration
        if (!_isMenuOpen) 
        {
            StartCoroutine(HidePanelAfterAnimation());
        }
    }

    private IEnumerator HidePanelAfterAnimation()
    {
        yield return new WaitForSeconds(1f); // Adjust based on "SlideIn" animation length
        overlayPanel.SetActive(false); // Hide panel after animation completes
    }


}
