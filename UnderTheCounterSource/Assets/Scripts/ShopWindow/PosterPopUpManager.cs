using Technical;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShopWindow
{
    public class PosterPopUpManager : MonoBehaviour
    {
        public GameObject popUpWindow; // The pop-up window

        // UI Elements in the pop-up
        public Image posterImageUI;       // Image on the left panel
        public TMP_Text posterNameUI;         // Text for the poster name
        public TMP_Text posterDescriptionUI;  // Text for the description
        public TMP_Text buffTextUI;           // Text for the buff
        public TMP_Text nerfTextUI;           // Text for the nerf
        public TMP_Text priceTextUI;          // Text for the price
        public Image currencyIconUI;      // Image for the currency icon

        // Method to display the pop-up with the provided details
        public void ShowPosterDetails(Sprite image, string name, string description, string buff, string nerf, string price, Sprite currency)
        {
            // Set the UI elements
            if (posterImageUI != null) posterImageUI.sprite = image;
            if (posterNameUI != null) posterNameUI.text = name;
            if (posterDescriptionUI != null) posterDescriptionUI.text = description;
            if (buffTextUI != null) buffTextUI.text = $"<size=150%><b>+</b><size=100%>  {buff}";
            if (nerfTextUI != null) nerfTextUI.text = $"<size=150%><b>-</b><size=100%>  {nerf}";
            if (priceTextUI != null) priceTextUI.text = price == "Owned" ? price : $"{price}$";
            if (currencyIconUI != null)
            {
                if (currency != null)
                {
                    currencyIconUI.sprite = currency;
                    currencyIconUI.gameObject.SetActive(true);
                }
                else currencyIconUI.gameObject.SetActive(false);
                
            }

            // Activate the pop-up
            if (popUpWindow != null)
            {
                GetComponent<FadeCanvas>().FadeIn();
            }
        }

        // Method to close the pop-up
        public void ClosePopUp()
        {
            if (popUpWindow != null)
            {
                GetComponent<FadeCanvas>().FadeOut();
            }
        }
    }
}