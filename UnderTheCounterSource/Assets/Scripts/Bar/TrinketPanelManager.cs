using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AYellowpaper.SerializedCollections.Editor.Data;
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
            EventSystemManager.OnPosterObtained += DisplayPoster; // New event for posters
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            EventSystemManager.OnTrinketDisplayed -= DisplayTrinket;
            EventSystemManager.OnPosterObtained -= DisplayPoster;
        }

        private void LoadTrinkets()
        {
            string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/TrinketData/Trinkets.json");
            _trinkets = JsonConvert.DeserializeObject<TrinketList>(jsonString).trinkets;
        }

        private void LoadPosters()
        {
            string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/UnlockedPosterData/Posters.json");
            _posters = JsonConvert.DeserializeObject<PosterList>(jsonString).posters;
            Debug.Log(_posters);
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
            Debug.Log("Displaying poster");
            _isPoster = true; // This is a poster
            UnlockedPoster poster = _posters.Find(a => a.id == id);
            if (poster != null)
            {
                ShowItem(poster.filename, poster.caption, poster.id, "Poster");
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
                // Load the poster as a Sprite
                string posterPath = $"Sprites/Posters and more/{prefabName}"; // Without ".png", Unity adds it automatically
                Sprite posterSprite = Resources.Load<Sprite>(posterPath);

                if (posterSprite != null)
                {
                    // Create an Image object to display the poster
                    GameObject newPoster = new GameObject(prefabName);
                    newPoster.transform.SetParent(spawn, false); // Set the spawn as parent

                    Image imageComponent = newPoster.AddComponent<Image>();
                    imageComponent.sprite = posterSprite;

                    // Double the size of the original sprite
                    RectTransform rectTransform = newPoster.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(500, 625); // Expand the size
                    rectTransform.localScale = Vector3.one; // Ensure the scale is 1:1
                }
                else
                {
                    Debug.LogError($"Poster image '{prefabName}' not found at '{posterPath}'");
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
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            hangButton.SetActive(false);

            // Destroy the displayed trinket or poster
            string tagToFind = _isPoster ? "Poster" : "Trinket";
            GameObject displayedItem = GameObject.FindGameObjectWithTag(tagToFind);
            if (displayedItem != null)
            {
                Destroy(displayedItem);
            }
            
            // Trigger the appropriate event
            if (_isPoster)
            {
                EventSystemManager.OnPosterObtained(_displayedId);
            }
            else
            {
                EventSystemManager.OnTrinketObtained(_displayedId);
            }
        }

        private void ShowTrinketArea()
        {
            _canvasGroup.blocksRaycasts = true;
            StartCoroutine(WaitBeforeShowingTrinketArea());
        }

        private IEnumerator WaitBeforeShowingTrinketArea()
        {
            yield return new WaitForSeconds(1f);
            
            gameObject.GetComponent<FadeCanvas>().FadeIn();
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
        public string filename;
        public string caption;
    }

    [Serializable]
    public class PosterList
    {
        public List<UnlockedPoster> posters;
    }
}
