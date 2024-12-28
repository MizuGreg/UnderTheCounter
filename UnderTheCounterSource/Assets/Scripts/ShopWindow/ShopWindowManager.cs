using System.Collections;
using System.Collections.Generic;
using System.IO;
using Bar;
using Technical;
using TMPro;
using Tutorial;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ShopWindow
{
    public class ShopWindowManager : MonoBehaviour
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
            if (forceDay != 0) Day.CurrentDay = forceDay;
            #endif
            
            dayText.text = $"DAY {Day.CurrentDay}";
            savingsText.text = $"${Day.Savings:N0}";
            if (Day.CurrentDay == 2) StartCoroutine(WaitAndStartTutorial());
            if (Day.CurrentDay == 3) newspaper.SetActive(true);
            else newspaper.SetActive(false);

            LoadPosters();
        }

        private void LoadPosters()
        {
            // Step 1: Load the JSON file
            string filePath = Path.Combine(Application.persistentDataPath, "posters.json");
            if (!File.Exists(filePath))
            {
                Debug.LogWarning("No saved posters data found.");
                return;
            }

            string json = File.ReadAllText(filePath);
            PosterDataListWrapper posterDataList = JsonUtility.FromJson<PosterDataListWrapper>(json);

            // Step 2: Get all poster prefab scripts from canvasContainer
            PosterPrefabScript[] posterPrefabs = canvasContainer.GetComponentsInChildren<PosterPrefabScript>();

            // Step 3: Iterate through each poster in the prefab scripts
            foreach (PosterPrefabScript pps in posterPrefabs)
            {
                PosterData matchingPosterData = posterDataList.posters.Find(p => p.id == pps.posterID);
                if (matchingPosterData == null)
                {
                    Debug.LogWarning($"No data found for poster ID: {pps.posterID}");
                    continue;
                }

                // Step 4: Update the poster's price
                pps.posterPrice = matchingPosterData.price;
                if (pps.posterPrice < 0) pps.isLocked = false;
                pps.UpdateUI();

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
            // Step 1: Create a list to store poster data
            List<PosterData> posterDataList = new List<PosterData>();

            // Step 2: Loop through all posters in Day.Posters
            foreach (Poster poster in Day.CurrentPosters)
            {
                // Copy important variables to a PosterData object
                PosterData data = new PosterData
                {
                    id = poster.posterID,
                    price = poster.price,
                    hanged = poster.hanged
                };

                posterDataList.Add(data);
            }

            // Step 3: Convert the list to JSON
            string json = JsonUtility.ToJson(new PosterDataListWrapper { posters = posterDataList }, true);

            // Step 4: Save JSON to a file
            string filePath = Path.Combine(Application.persistentDataPath, "posters.json");
            File.WriteAllText(filePath, json);

            Debug.Log("Posters saved to: " + filePath);
        }

        // Serializable class to store poster data
        [System.Serializable]
        public class PosterData
        {
            public int id;
            public float price;
            public int hanged;
        }

        // Wrapper for the list of posters
        [System.Serializable]
        public class PosterDataListWrapper
        {
            public List<PosterData> posters;
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

        public void AddPoster(int posterID)
        {
            if (!Day.IsPosterActive(posterID))
            {
                Day.CurrentPosters.Add(new Poster(posterID, null, null, 0, null, null, null, 0));
            }
        }

        public void AddPoster(Poster poster)
        {
            if (!Day.IsPosterActive(poster.posterID))
            {
                Day.CurrentPosters.Add(poster);
            }
        }

        public void UpdatePoster(Poster poster)
        {
            // Step 1: Find the index of the existing poster with the same ID
            for (int i = 0; i < Day.CurrentPosters.Count; i++)
            {
                if (Day.CurrentPosters[i].posterID == poster.posterID)
                {
                    // Step 2: Replace the existing poster with the new one
                    Day.CurrentPosters[i] = poster;
                    return; // Exit function since the poster was replaced
                }
            }
            // Step 3: If not found, add the new poster to the list
            AddPoster(poster);
        }
    }
}