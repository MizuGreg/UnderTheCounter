using System.Collections;
using System.Collections.Generic;
using System.IO;
using Bar;
using Newtonsoft.Json;
using SavedGameData;
using Technical;
using TMPro;
using Tutorial;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ShopWindow
{
    public partial class ShopWindowManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasContainer;
        [SerializeField] private TextMeshProUGUI dayText;
        [SerializeField] private TextMeshProUGUI savingsText;
        private TutorialManager2 tutorialManager2;
        [SerializeField] private GameObject newspaper;
        
        [SerializeField] public int forceDay = 2;

        private void Start()
        {
            EventSystemManager.OnLoadShopWindow();
            
            canvasContainer.GetComponent<FadeCanvas>().FadeIn();
            tutorialManager2 = GetComponent<TutorialManager2>();
            
            #if UNITY_EDITOR
            if (forceDay != 0) GameData.CurrentDay = forceDay;
            #endif
            
            dayText.text = $"DAY {GameData.CurrentDay}";
            savingsText.text = $"${GameData.Savings:N0}";
            if (GameData.CurrentDay == 2) StartCoroutine(WaitAndStartTutorial());
            if (GameData.CurrentDay == 3) newspaper.SetActive(true);
            else newspaper.SetActive(false);

            LoadPosters();
        }

        private void LoadPosters()
        {
            // Step 1: fetch poster data
            List<PosterData> posterDataList = GameData.Posters;

            // Step 2: Get all poster prefab scripts from canvasContainer
            PosterPrefabScript[] posterPrefabs = canvasContainer.GetComponentsInChildren<PosterPrefabScript>(true);

            // Step 3: Iterate through each poster in the prefab scripts
            foreach (PosterPrefabScript pps in posterPrefabs)
            {
                PosterData matchingPosterData = posterDataList.Find(p => p.id == pps.posterID);
                if (matchingPosterData == null)
                {
                    Debug.LogWarning($"No data found for poster ID: {pps.posterID}. PPS will fallback to default values.");
                    continue;
                }

                // Step 4: Update the poster's price
                pps.posterPrice = matchingPosterData.price;
                if (pps.posterPrice < 0) pps.isLocked = false;
                pps.UpdateUI();
                
                // Set poster visible/invisible
                pps.gameObject.SetActive(matchingPosterData.visible);

                // Step 5: Handle hanged logic
                if (matchingPosterData.hanged != 0)
                {
                    // Find DropTarget components
                    DropTarget[] dropTargets = canvasContainer.GetComponentsInChildren<DropTarget>();
                    DropTarget targetDrop = null;

                    if (matchingPosterData.hanged == 1)
                    {
                        targetDrop = dropTargets[0];
                    }
                    else if (matchingPosterData.hanged == 2)
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
                        Debug.Log(targetDrop);
                        pps.TogglePosterDetails(false);
                        dragHandler.HangPoster(validArea, targetDrop, pps);
                    }
                    else
                    {
                        Debug.LogError($"DropTarget with ID {matchingPosterData.hanged} not found.");
                    }
                }
            }
        }
        
        public void SavePosters()
        {
            PosterPrefabScript[] posterPrefabs = canvasContainer.GetComponentsInChildren<PosterPrefabScript>(true);
            List<PosterData> posterDataList = new List<PosterData>();
            foreach (PosterPrefabScript pps in posterPrefabs)
            {
                posterDataList.Add(new PosterData(pps.posterID, pps.posterPrice, pps.hanged, pps.isActiveAndEnabled));
            }
            GameData.Posters = posterDataList;
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