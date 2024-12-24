using System.Globalization;
using Bar;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShopWindow
{
    public class PosterPrefabScript : MonoBehaviour
    {
        [SerializeField] private ShopWindowManager shopWindowManager;
        
        public int posterID;
        public Sprite posterImage; // Reference to the image component for the poster
        public string posterNameText; // Reference to the text component for the poster name
        public float posterPrice; // Reference to the poster price
        public string posterBuff; // Reference to the poster buff percentage
        public string posterNerf; // Reference to the poster nerf percentage
        public string posterDescription; // Reference to the text for the poster description
        public Sprite currencyIcon;    // Assign the currency icon
    
        public GameObject posterPopUpPrefab; // Reference to the shared PosterPopUp prefab in the scene
        
        public bool isLocked; // Indicates if the poster is locked
    
        private bool _isDragging; // Track whether the item is being dragged
        
        [SerializeField] private Image posterImageUI; // Reference to the Image UI component for poster
        [SerializeField] private CanvasGroup canvasGroup; // Reference for fading effect

        // Method to set poster data
        public void SetPosterData(Poster poster)
        {
            posterImage = poster.image;
            posterNameText = poster.name;
            posterPrice = poster.price;
            posterBuff = poster.buff;
            posterNerf = poster.nerf;
            posterDescription = poster.description;
        }
        
        private void Awake()
        {
            // If posterImageUI isn't already assigned, try to find it
            if (posterImageUI == null)
            {
                posterImageUI = transform.Find("PosterImage")?.GetComponent<Image>();
            }

            // Ensure that posterImageUI is found, then get the CanvasGroup
            if (posterImageUI != null)
            {
                canvasGroup = posterImageUI.GetComponent<CanvasGroup>();

                // If CanvasGroup is not assigned, add it
                if (canvasGroup == null)
                {
                    canvasGroup = posterImageUI.gameObject.AddComponent<CanvasGroup>();
                }
            }
            else
            {
                Debug.LogWarning("posterImageUI not found! Make sure it's assigned or the hierarchy is correct.");
            }
            
            // Initialize UI based on poster values
            UpdateUI();
        }
        

        public void UpdateUI()
        {
            // Update the poster's locked state
            SetLocked(isLocked);
            
            // Find the "posterImage" child first
            var posterImageTransform = transform.Find("PosterImage");
            if (posterImageTransform == null) return;
            // Find the "posterPrice" child under "posterImage"
            var posterPriceTransform = posterImageTransform.Find("PosterPrice");
            if (posterPriceTransform == null) return;
            // Get the TextMeshProUGUI component
            var priceTextUI = posterPriceTransform.GetComponent<TextMeshProUGUI>();
            if (priceTextUI != null)
            {
                // Update the text based on the poster price
                priceTextUI.text = posterPrice >= 0 
                    ? $"{posterPrice.ToString(CultureInfo.InvariantCulture)}$" 
                    : "Owned";
            }
            else
            {
                Debug.LogWarning("TextMeshProUGUI component not found on posterPrice.");
            }

            // Find the "PriceIcon" child under "posterPrice"
            var priceIconTransform = posterPriceTransform.Find("PriceIcon");
            if (priceIconTransform != null)
            {
                // Activate or deactivate the PriceIcon based on poster price
                priceIconTransform.gameObject.SetActive(posterPrice >= 0);
            }
        }

        private void SetLocked(bool locked)
        {
            isLocked = locked;

            // Update the UI to reflect the locked state
            if (canvasGroup == null) return;
            Debug.Log("canvas found");
            canvasGroup.alpha = locked ? 0.5f : 1f; // Fade effect for locked posters
            canvasGroup.interactable = !locked; // Disable interaction
            canvasGroup.blocksRaycasts = !locked; // Prevent clicks on locked posters
        }
    
        //Hide or Show Poster Price for when in placeholder or when in menu
        public void TogglePosterDetails(bool show)
        {
            foreach (var child in transform.GetComponentsInChildren<Transform>(true))
            {
                // Check if the GameObject has the "PosterDetail" tag
                if (child.CompareTag("PosterDetail"))
                {
                    // Enable or disable the entire GameObject
                    child.gameObject.SetActive(show);
                }
            }
        }
    
        public void OnPosterClicked()
        {
            if (_isDragging ) return; // Only show popup if no drag is in progress

            // Check if the poster price is less than 0
            var displayPrice = posterPrice.ToString(CultureInfo.InvariantCulture);
            var displayIcon = currencyIcon;

            if (float.TryParse(posterPrice.ToString(CultureInfo.InvariantCulture), out var priceValue) && priceValue < 0)
            {
                displayPrice = "    Owned"; // Set to "Owned" if the price is less than 0
                displayIcon = null;     // Remove the currency icon
            }

            // Enable the pop-up
            posterPopUpPrefab.SetActive(true);

            // Access the pop-up's components and update its content
            var popUpScript = posterPopUpPrefab.GetComponent<PosterPopUpManager>();
            if (popUpScript != null)
            {
                Debug.Log(this.GetType());
                popUpScript.SetCurrentPoster(this);
                popUpScript.ShowPosterDetails(
                    posterImage,
                    posterNameText,
                    posterDescription,
                    posterBuff,
                    posterNerf,
                    displayPrice, // Use the modified display price
                    displayIcon   // Use the modified currency icon
                );
            }
        }

        // Method to set isDragging flag when dragging starts
        public void SetIsDragging(bool value)
        {
            _isDragging = value;
        }

        public void AddPosterToHungPosters()
        {
            shopWindowManager.AddPoster(posterID);
        }

        public void RemovePosterFromHungPosters()
        {
            shopWindowManager.RemovePoster(posterID);
        }

        public void BuyPoster()
        {
            isLocked = false;
            posterPrice = -1; // Mark as owned
            UpdateUI(); // Refresh the poster UI to show "Owned"
        }
    }
}