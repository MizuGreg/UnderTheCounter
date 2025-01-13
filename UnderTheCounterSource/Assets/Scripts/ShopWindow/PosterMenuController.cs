using UnityEngine;
using UnityEngine.UI;

namespace ShopWindow
{
    public class PosterMenuController : MonoBehaviour
    {
        public GameObject[] submenus; // Array to hold the 3 submenus
        public Image[] bullets; // Array to hold the 3 bullet images
        public Button leftArrow; // Reference to the left arrow button
        public Button rightArrow; // Reference to the right arrow button
        public Sprite activeSprite;
        public Sprite inactiveSprite;

        private int _currentMenuIndex = 0; // Tracks the currently active submenu

        private void Start()
        {
            // Initialize by showing the first submenu
            UpdateMenu();
        }

        // Method to show the next submenu
        public void ShowNextMenu()
        {
            _currentMenuIndex = (_currentMenuIndex + 1) % submenus.Length;
            UpdateMenu();
        }

        // Method to show the previous submenu
        public void ShowPreviousMenu()
        {
            _currentMenuIndex = (_currentMenuIndex - 1 + submenus.Length) % submenus.Length;
            UpdateMenu();
        }

        public void ShowMenu(int index)
        {
            _currentMenuIndex = index;
            UpdateMenu();
        }

        // Method to update the active submenu and bullets
        private void UpdateMenu()
        {
            // Enable the current submenu and disable others
            for (int i = 0; i < submenus.Length; i++)
            {
                submenus[i].SetActive(i == _currentMenuIndex);
            }

            // Update the bullets by changing the sprite
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i].sprite = (i == _currentMenuIndex) ? activeSprite : inactiveSprite;
            }
            
            // Hide or show navigation arrows depending on the page
            leftArrow.gameObject.SetActive(_currentMenuIndex != 0);
            rightArrow.gameObject.SetActive(_currentMenuIndex != submenus.Length - 1);
        }
    }
}