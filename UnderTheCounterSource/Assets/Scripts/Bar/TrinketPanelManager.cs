using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Technical;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bar
{
    public class TrinketPanelManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI caption;
        [SerializeField] private RectTransform spawn;
        [SerializeField] private GameObject hangButton;

        private CanvasGroup _canvasGroup;
        private List<Trinket> _trinkets;
        private List<UnlockedPoster> _posters; // New list for posters
        private int _displayedId;
        private bool _isPoster; // Tracks whether the displayed item is a poster

        private void Start()
        {
            _canvasGroup = gameObject.GetComponent<CanvasGroup>();
            
            // Load trinkets and posters from JSON
            LoadTrinkets();
            LoadPosters();
            
            // Subscribe to events
            EventSystemManager.OnTrinketDisplayed += DisplayTrinket;
            // EventSystemManager.OnPosterObtained += DisplayPoster; // New event for posters
            EventSystemManager.OnPosterDisplayed += DisplayPoster;
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            EventSystemManager.OnTrinketDisplayed -= DisplayTrinket;
            // EventSystemManager.OnPosterObtained -= DisplayPoster;
            EventSystemManager.OnPosterDisplayed -= DisplayPoster;
        }

        private void LoadTrinkets()
        {
            string jsonString = Resources.Load<TextAsset>("TextAssets/TrinketData/Trinkets").text;
            _trinkets = JsonConvert.DeserializeObject<TrinketList>(jsonString).trinkets;
        }

        private void LoadPosters()
        {
            string jsonString = Resources.Load<TextAsset>("TextAssets/UnlockedPosterData/Posters").text;
            _posters = JsonConvert.DeserializeObject<PosterList>(jsonString).posters;
        }

        private void DisplayTrinket(int id)
        {
            _isPoster = false; // This is a trinket
            Trinket trinket = _trinkets.Find(a => a.id == id);
            if (trinket != null)
            {
                ShowItem(trinket.title, trinket.caption, trinket.id, "Trinket");
            }
            else
            {
                Debug.LogWarning($"Trinket with ID '{id}' not found");
            }
        }

        private void DisplayPoster(int id)
        {
            Debug.Log("Display poster is called");
            _isPoster = true; // This is a poster
            UnlockedPoster poster = _posters.Find(a => a.id == id);
            if (poster != null)
            {
                ShowItem(poster.title, poster.caption, poster.id, "Poster");
            }
            else
            {
                Debug.LogWarning($"Poster with ID '{id}' not found");
            }
        }

        private void ShowItem(string prefabName, string captionText, int id, string tag)
        {
            // Set the caption
            caption.text = captionText;

            if (_isPoster)
            {
                // Load the trinket prefab
                string prefabDirectory = $"Prefabs/Unlocked{tag}s/{prefabName}";
                GameObject prefab = Resources.Load<GameObject>(prefabDirectory);
                

                if (prefab != null)
                {
                    Debug.Log(prefab);
                    GameObject newPoster = Instantiate(prefab, spawn);
                }
                else
                {
                    Debug.LogError($"Prefab '{prefabName}' not found at '{prefabDirectory}'");
                }
            }
            else
            {
                // Load the trinket prefab
                string prefabDirectory = $"Prefabs/{tag}s/{prefabName}";
                GameObject prefab = Resources.Load<GameObject>(prefabDirectory);

                if (prefab != null)
                {
                    GameObject newTrinket = Instantiate(prefab, spawn);
                }
                else
                {
                    Debug.LogError($"Prefab '{prefabName}' not found at '{prefabDirectory}'");
                }
            }

            // Set the displayed item's ID
            _displayedId = id;

            // Show the UI
            ShowTrinketArea();
        }



        public void HideTrinketArea()
        {
            GetComponent<FadeCanvas>().FadeOut();
            hangButton.SetActive(false);

            // Destroy the displayed trinket or poster
            string tagToFind = _isPoster ? "Poster" : "Trinket";
            GameObject displayedItem = GameObject.FindGameObjectWithTag(tagToFind);
            if (displayedItem != null)
            {
                Destroy(displayedItem);
            }
            
            // Trigger the appropriate event
            if (!_isPoster)
            {
                EventSystemManager.OnTrinketObtained(_displayedId);
            }
        }

        private void ShowTrinketArea()
        {
            gameObject.SetActive(true);
            StartCoroutine(WaitBeforeShowingTrinketArea());
        }

        private IEnumerator WaitBeforeShowingTrinketArea()
        {
            yield return new WaitForSeconds(1f);
            
            GetComponent<FadeCanvas>().FadeIn();
            _canvasGroup.blocksRaycasts = true;
            hangButton.SetActive(true);
        }
    }

    // Classes for Trinkets and Posters
    [Serializable]
    public class UnlockedPoster
    {
        public int id;
        public string title;
        public string caption;
    }

    [Serializable]
    public class PosterList
    {
        public List<UnlockedPoster> posters;
    }
}
