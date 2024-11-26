using UnityEngine;
using UnityEngine.UI;

public class PosterMenuController : MonoBehaviour
{
    public GameObject[] submenus; // Array to hold the 3 submenus
    public Image[] bullets; // Array to hold the 3 bullet images
    public Button leftArrow; // Reference to the left arrow button
    public Button rightArrow; // Reference to the right arrow button

    private int currentMenuIndex = 0; // Tracks the currently active submenu

    void Start()
    {
        // Initialize by showing the first submenu
        UpdateMenu();
    }

    // Method to show the next submenu
    public void ShowNextMenu()
    {
        currentMenuIndex = (currentMenuIndex + 1) % submenus.Length;
        UpdateMenu();
    }

    // Method to show the previous submenu
    public void ShowPreviousMenu()
    {
        currentMenuIndex = (currentMenuIndex - 1 + submenus.Length) % submenus.Length;
        UpdateMenu();
    }

    // Method to update the active submenu and bullets
    private void UpdateMenu()
    {
        // Enable the current submenu and disable others
        for (int i = 0; i < submenus.Length; i++)
        {
            submenus[i].SetActive(i == currentMenuIndex);
        }

        // Update the bullets (white for active, black for inactive)
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].color = (i == currentMenuIndex) ? Color.white : Color.black;
        }
    }
}