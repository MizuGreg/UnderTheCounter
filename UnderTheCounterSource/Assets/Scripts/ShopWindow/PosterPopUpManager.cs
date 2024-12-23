using Bar;
using Technical;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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
        [SerializeField] private GameObject insufficientMoneyPopup;
        [SerializeField] private GameObject confirmPurchasePopup;
        private PosterPrefabScript _currentPoster; // Track the current poster for buying
        public Button confirmBuyButton; // Confirm buy button
        

        // Method to display the pop-up with the provided details
        public void ShowPosterDetails(Sprite image, string name, string description, string buff, string nerf, string price, Sprite currency)
        {
            // Set the UI elements
            if (posterImageUI != null) posterImageUI.sprite = image;
            if (posterNameUI != null) posterNameUI.text = name;
            if (posterDescriptionUI != null) posterDescriptionUI.text = description;
            if (buffTextUI != null) buffTextUI.text = $"<size=150%><b>+</b><size=100%>  {buff}";
            if (nerfTextUI != null) nerfTextUI.text = $"<size=150%><b>-</b><size=100%>  {nerf}";
            if (priceTextUI != null) priceTextUI.text = price == "    Owned" ? price : $"{price}$";
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
            if (popUpWindow == null) return;
            GetComponent<FadeCanvas>().FadeOut();
            CloseConfirmPurchasePopup();
            CloseInsufficientMoneyPopup();
        }

        private void ShowInsufficientMoneyPopup()
        {
            insufficientMoneyPopup.transform.parent.gameObject.SetActive(true);
            insufficientMoneyPopup.SetActive(true);
        }

        // Hiding the popup
        public void CloseInsufficientMoneyPopup()
        {
            insufficientMoneyPopup.transform.parent.gameObject.SetActive(false);
            insufficientMoneyPopup.SetActive(false);
        }

        private void ShowConfirmPurchasePopup(PosterPrefabScript poster)
        {
            confirmPurchasePopup.transform.parent.gameObject.SetActive(true);
            confirmPurchasePopup.SetActive(true);
            confirmBuyButton.onClick.RemoveAllListeners(); // Clear previous listeners
            confirmBuyButton.onClick.AddListener(() => ConfirmPurchase(poster)); // Add new listener
        }

        // Hiding the popup
        private void CloseConfirmPurchasePopup()
        {
            confirmPurchasePopup.transform.parent.gameObject.SetActive(false);
            confirmPurchasePopup.SetActive(false);
        }
        
        // Method to set the current poster when opening the popup
        public void SetCurrentPoster(PosterPrefabScript poster)
        {
            _currentPoster = poster;
        }
        
        // Called when the Buy button is clicked in the popup
        public void OnBuyButtonClicked()
        {
            Debug.Log("Buy Button clicked");
            if (_currentPoster == null || _currentPoster.posterPrice < 0) return;

            if (_currentPoster.posterPrice > Day.Savings)
            {
                // Show insufficient money popup
                ShowInsufficientMoneyPopup();
            }
            else
            {
                // Show confirmation popup
                ShowConfirmPurchasePopup(_currentPoster);
            }
        }
        
        private void ConfirmPurchase(PosterPrefabScript poster)
        {
            // Deduct money and mark the poster as purchased
            Day.Savings -= poster.posterPrice;
            ClosePopUp();
            poster.isLocked = false;
            poster.posterPrice = -1; // Mark as owned
            poster.UpdateUI(); // Refresh the poster UI to show "Owned"

            // Hide the confirmation popup
            confirmPurchasePopup.SetActive(false);

            // Optionally update player's money UI here
            Debug.Log($"Purchased {poster.posterNameText}. Remaining Money: {Day.Savings}$");
        }
    }
}