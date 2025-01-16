using System;
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
        private int _trinketId;

        private void Start()
        {
            _canvasGroup = gameObject.GetComponent<CanvasGroup>();
            
            // Load trinkets from JSON
            LoadTrinkets();
            
            // Subscribe to events
            EventSystemManager.OnTrinketDisplayed += DisplayTrinket;
            
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            EventSystemManager.OnTrinketDisplayed -= DisplayTrinket;
        }

        private void LoadTrinkets()
        {
            // Read Trinkets JSON and create trinkets list
            string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/TrinketData/Trinkets.json");
            
            _trinkets = JsonConvert.DeserializeObject<TrinketList>(jsonString).trinkets;
        }

        private void DisplayTrinket(int id)
        {
            // Find the trinket
            Trinket trinket = _trinkets.Find(a => a.id == id);
            
            if (trinket != null)
            {
                // Set trinket's caption
                caption.text = trinket.caption;
                
                // Instantiate trinket
                GameObject prefab = Resources.Load<GameObject>($"Prefabs/Trinkets/{trinket.title}");

                if (prefab != null)
                {
                    // Instantiate the prefab at the spawn position
                    GameObject newTrinket = Instantiate(prefab, spawn);
                }
                else
                {
                    Debug.LogError($"Prefab with name '{trinket.title}' not found");
                }
                
                // Set trinket's editor id
                _trinketId = trinket.id;
                
                ShowTrinketArea();
            }
            else
            {
                Debug.LogWarning($"Trinket with ID '{id}' not found");
            }
        }

        public void HideTrinketArea()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            hangButton.SetActive(false);

            GameObject displayedTrinket = GameObject.FindGameObjectWithTag("Trinket");
            if (displayedTrinket != null)
            {
                Destroy(displayedTrinket);
            }
            
            EventSystemManager.OnTrinketObtained(_trinketId);
        }

        private void ShowTrinketArea()
        {
            gameObject.GetComponent<FadeCanvas>().FadeIn();
            _canvasGroup.blocksRaycasts = true;
            hangButton.SetActive(true);
        }
        
    }
}