using UnityEngine;

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
    
        private bool _isDragging = false; // Track whether the item is being dragged

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
            if (_isDragging) return; // Only show popup if no drag is in progress
            // Enable the pop-up
            posterPopUpPrefab.SetActive(true);

            // Access the pop-up's components and update its content
            var popUpScript = posterPopUpPrefab.GetComponent<PosterPopUpManager>();
            if (popUpScript != null)
            {
                popUpScript.ShowPosterDetails(
                    posterImage,
                    posterNameText,
                    posterDescription,
                    posterBuff,
                    posterNerf,
                    posterPrice,
                    currencyIcon
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
    }
}