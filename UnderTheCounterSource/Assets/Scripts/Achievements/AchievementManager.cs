using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Technical;
using UnityEngine;

namespace Achievements
{
    public class AchievementManager : MonoBehaviour
    {
        [SerializeField] private GameObject popUp;
        
        private List<Achievement> _achievements;
       
        
        // DEBUG
        private void Update()
        {
            // Esempio: Incrementa il progresso dell'achievement "pressed W" quando premi il tasto W
            if (Input.GetKeyDown(KeyCode.W))
            {
                UpdateAchievement("pressed W", 1);
            }
        }
        // ------------------


        private void Start()
        {
            // Load achievements status
            LoadAchievements();
            
            // Debug
            //ResetAchievements();
            //PrintAchievements();
            
            // Subscribe to events
            EventSystemManager.OnAchievementProgress += UpdateAchievement;
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            EventSystemManager.OnAchievementProgress -= UpdateAchievement;
        }

        private void LoadAchievements()
        {
            // Read Achievements JSON and create achievements list
            string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/AchievementData/Achievements.json");
            
            _achievements = JsonConvert.DeserializeObject<AchievementList>(jsonString).achievements;
        }

        private void UpdateAchievement(string id, int progress)
        {
            // Find the achievement
            Achievement achievement = _achievements.Find(a => a.id == id);

            if (achievement != null && !achievement.isUnlocked)
            {
                // Update the progress
                achievement.progress += progress;

                // Check if the target has been reached
                if (achievement.progress >= achievement.target)
                {
                    achievement.isUnlocked = true;
                    Debug.Log($"Achievement Unlocked: {achievement.title}");
                    popUp.GetComponent<DisplayAchievement>().DisplayPopUp();
                    popUp.GetComponent<DisplayAchievement>().SetPopUpValues(achievement.title,achievement.description);
                }

                // Save updated progresses in the JSON
                SaveAchievements();
            }
            else
            {
                Debug.LogWarning($"Achievement with ID '{id}' not found or already unlocked.");
            }
        }
        
        private void SaveAchievements()
        {
            // Serialize achievements in JSON format
            string jsonString = JsonConvert.SerializeObject(new AchievementList { achievements = _achievements }, Formatting.Indented);

            // Write in the JSON
            File.WriteAllText(Application.streamingAssetsPath + "/AchievementData/Achievements.json", jsonString);
    
            Debug.Log("Achievements saved successfully.");
        }

        private void ResetAchievements()
        {
            foreach (var a in _achievements)
            {
                a.isUnlocked = false;
                a.progress = 0;
            }
            
            SaveAchievements();
        }

        private void PrintAchievements()
        {
            foreach (var a in _achievements)
            {
                print(a);
            }
        }
    }
}
