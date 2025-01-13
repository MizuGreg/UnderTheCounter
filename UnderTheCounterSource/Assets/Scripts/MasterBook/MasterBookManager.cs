using Technical;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace MasterBook
{
    public class MasterBookManager : MonoBehaviour
    {
        private RecipeTabManager recipeTabManager;
        private LogTabManager logTabManager;
        private PosterTabManager posterTabManager;
        
        [SerializeField] private FadeCanvas masterBook;

        [SerializeField] private Button recipeButton;
        [SerializeField] private Button logButton;
        [SerializeField] private Button posterButton;

        private Vector3 originalRecipeButtonPosition;
        private Vector3 originalLogButtonPosition;
        private Vector3 originalPosterButtonPosition;
        private Vector3 selectedRecipeButtonPosition;
        private Vector3 selectedLogButtonPosition;
        private Vector3 selectedPosterButtonPosition;
        private float buttonPositionShift = -25f;

        private Canvas currentlyOpenedPage;
        
        [SerializeField] private Canvas recipePage;
        [SerializeField] private Canvas logPage;
        [SerializeField] private Canvas posterPage;

        private void Awake()
        {
            logTabManager = GetComponent<LogTabManager>();
            posterTabManager = GetComponent<PosterTabManager>();
        }
        
        private void Start()
        {
            originalRecipeButtonPosition = recipeButton.transform.localPosition;
            originalLogButtonPosition = logButton.transform.localPosition;
            originalPosterButtonPosition = posterButton.transform.localPosition;
            selectedRecipeButtonPosition = originalRecipeButtonPosition + new Vector3(buttonPositionShift, 0, 0);
            selectedLogButtonPosition = originalLogButtonPosition + new Vector3(buttonPositionShift, 0, 0);
            selectedPosterButtonPosition = originalPosterButtonPosition + new Vector3(buttonPositionShift, 0, 0);
            
            currentlyOpenedPage = null;

            logTabManager.SetDay();
        }
        
        public void OpenMasterBook()
        {
            if (masterBook.GetComponent<FadeCanvas>().IsFading()) return;
            EventSystemManager.OnMasterBookOpened();
            masterBook.GetComponent<FadeCanvas>().FadeIn();
            
            if (currentlyOpenedPage == null) OpenRecipeTab(true); // opens recipe tab by default
        }

        public void CloseMasterBook()
        {
            if (masterBook.GetComponent<FadeCanvas>().IsFading()) return;
            EventSystemManager.OnMasterBookClosed();
            masterBook.GetComponent<FadeCanvas>().FadeOut();
        }

        public bool IsMasterBookOpen()
        {
            return masterBook.gameObject.activeSelf;
        }

        public void OpenRecipeTab(bool silent = false)
        {
            if (currentlyOpenedPage == recipePage) return; // stops flow early if the tab button is pressed more than once
            currentlyOpenedPage = recipePage;
            if (!silent) EventSystemManager.OnTabChanged(); // for silently setting the tab when the master book is opened for the first time
            
            recipePage.gameObject.SetActive(true);
            logPage.gameObject.SetActive(false);
            posterPage.gameObject.SetActive(false);

            recipeButton.GetComponent<RectTransform>().localPosition = selectedRecipeButtonPosition;
            logButton.GetComponent<RectTransform>().localPosition = originalLogButtonPosition;
            posterButton.GetComponent<RectTransform>().localPosition = originalPosterButtonPosition;
        }

        public void OpenLogTab(bool silent = false)
        {
            logTabManager.PopulateLogPages(); // refreshes log tab
            if (currentlyOpenedPage == logPage) return; // then stops flow early if the tab button is pressed more than once
            currentlyOpenedPage = logPage;
            if (!silent) EventSystemManager.OnTabChanged();
            
            recipePage.gameObject.SetActive(false);
            logPage.gameObject.SetActive(true);
            posterPage.gameObject.SetActive(false);
            
            recipeButton.GetComponent<RectTransform>().localPosition = originalRecipeButtonPosition;
            logButton.GetComponent<RectTransform>().localPosition = selectedLogButtonPosition;
            posterButton.GetComponent<RectTransform>().localPosition = originalPosterButtonPosition;
            
            logTabManager.PopulateLogPages(); // refreshes log tab
        }

        public void OpenPosterTab()
        {
            posterTabManager.PopulatePosters();
            if (currentlyOpenedPage == posterPage) return; // stops flow early if the tab button is pressed more than once
            currentlyOpenedPage = posterPage;
            EventSystemManager.OnTabChanged();
            
            recipePage.gameObject.SetActive(false);
            logPage.gameObject.SetActive(false);
            posterPage.gameObject.SetActive(true);
            
            recipeButton.GetComponent<RectTransform>().localPosition = originalRecipeButtonPosition;
            logButton.GetComponent<RectTransform>().localPosition = originalLogButtonPosition;
            posterButton.GetComponent<RectTransform>().localPosition = selectedPosterButtonPosition;
        }
    }
}