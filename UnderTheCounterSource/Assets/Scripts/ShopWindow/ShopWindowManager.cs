using System.Collections;
using System.Collections.Generic;
using SavedGameData;
using Technical;
using TMPro;
using Tutorial;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ShopWindow
{
    public class ShopWindowManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasContainer;
        [SerializeField] private TextMeshProUGUI dayText;
        [SerializeField] private TextMeshProUGUI savingsText;
        private TutorialManager2 tutorialManager2;
        
        [SerializeField] public int forceDay;

        private void Start()
        {
            EventSystemManager.OnPosterBought += UpdateSavings;
            
            EventSystemManager.OnLoadShopWindow();
            
            canvasContainer.GetComponent<FadeCanvas>().FadeIn();
            tutorialManager2 = GetComponent<TutorialManager2>();
            
            #if UNITY_EDITOR
            if (forceDay != 0) GameData.CurrentDay = forceDay;
            #endif
            
            dayText.text = $"DAY {GameData.CurrentDay}";
            UpdateSavings();
            
            CheckDailyPapers();

            LoadPosters();
        }

        private void CheckDailyPapers()
        {
            if (GameData.CurrentDay == 2) StartCoroutine(WaitAndStartTutorial());
        }
        
        private void OnDestroy()
        {
            EventSystemManager.OnPosterBought -= UpdateSavings;
        }

        private void UpdateSavings()
        {
            savingsText.text = $"${GameData.Savings:N0}";
        }

        private void LoadPosters()
        {
            // Fetch poster data
            List<Poster> posterDataList = GameData.Posters;

            // Get all poster prefab scripts from canvasContainer
            PosterPrefabScript[] posterPrefabs = canvasContainer.GetComponentsInChildren<PosterPrefabScript>(true);
            
            // Iterate through each poster in the prefab scripts
            foreach (PosterPrefabScript pps in posterPrefabs)
            {
                Poster matchingPoster = posterDataList.Find(p => p.id == pps.posterID);
                if (matchingPoster == null)
                {
                    Debug.LogWarning($"No data found for poster ID: {pps.posterID}. PPS will fallback to default values.");
                    continue;
                }

                // Update the poster's price
                pps.posterPrice = matchingPoster.price;
                
                // Set poster visible/invisible
                pps.isVisible = matchingPoster.visible;
                
                // Set poster Info
                pps.posterBuff = matchingPoster.buff;
                pps.posterNerf = matchingPoster.nerf;
                pps.posterDescription = matchingPoster.description;
                pps.posterNameText = matchingPoster.name;
                pps.isLocked = matchingPoster.locked;
                
                // Handle hanged logic
                if (matchingPoster.hanged != 0)
                {
                    // Find DropTarget components
                    DropTarget[] dropTargets = canvasContainer.GetComponentsInChildren<DropTarget>();
                    DropTarget targetDrop = null;

                    if (matchingPoster.hanged == 1)
                    {
                        targetDrop = dropTargets[0];
                    }
                    else if (matchingPoster.hanged == 2)
                    {
                        targetDrop = dropTargets[1];
                    }

                    if (targetDrop != null)
                    {
                        // Perform the actual hanging
                        UIDragHandler dragHandler = pps.GetComponent<UIDragHandler>();
                        RectTransform validArea = targetDrop.GetComponentInParent<RectTransform>();
                        
                        if (dragHandler == null) Debug.LogError("Drag handler couldn't be found.");
                        if (validArea == null) Debug.LogError("validArea couldn't be found.");
                        
                        // Place the poster in the correct DropTarget
                        pps.TogglePosterDetails(false);
                        dragHandler.HangPoster(validArea, targetDrop, pps, true);
                    }
                    else
                    {
                        Debug.LogError($"DropTarget with ID {matchingPoster.hanged} not found.");
                    }
                }
                
                pps.UpdateUI();
            }
        }
        
        public void SavePosters()
        {
            PosterPrefabScript[] posterPrefabs = canvasContainer.GetComponentsInChildren<PosterPrefabScript>(true);
            List<Poster> posterList = new();
            foreach (PosterPrefabScript pps in posterPrefabs)
            {
                posterList.Add(new Poster(pps.posterID, pps.posterPrice, pps.hanged, pps.isLocked, pps.isVisible,
                    pps.posterNameText, pps.posterBuff, pps.posterNerf, pps.posterDescription));
            }
            GameData.Posters = posterList;
        }

        private IEnumerator WaitAndStartTutorial()
        {
            yield return new WaitForSeconds(1f);
            tutorialManager2.StartTutorial();
        }

        public void NextScene()
        {
            SavePosters();
            StartCoroutine(FadeThenNextScene());
        }

        private IEnumerator FadeThenNextScene()
        {
            canvasContainer.GetComponent<FadeCanvas>().FadeOut();
            yield return new WaitForSeconds(1.1f);
            SceneManager.LoadScene("Scenes/BarView");
        }
        
        public void BackToMainMenu()
        {
            canvasContainer.GetComponent<FadeCanvas>().FadeOut();
            StartCoroutine(WaitBeforeMenu());
        }
        
        private IEnumerator WaitBeforeMenu()
        {
            yield return new WaitForSeconds(1.1f);
            SceneManager.LoadScene("MainMenu");
        }
    }
}