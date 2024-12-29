using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Newtonsoft.Json;
using ShopWindow;
using UnityEngine;

namespace SavedGameData
{
    [SuppressMessage("ReSharper", "CheckNamespace")]
    public static class GameData
    {
        // Initialization
        public static GameLog Log = new();
        public static GameChoices Choices = new();
        
        public static string BarName = "The Chitchat";
        public static float DailyTime = 180;
        public static int CurrentDay = 1;
        public static int DrunkCustomers = 0;
        public static int MaxDrunkCustomers = 4;
    
        public static List<PosterData> Posters = new();
        public static List<int> Trinkets = new();
    
        public static float Savings = 100;
        public static float TodayEarnings = 0;
        public static int Rent = 20;
        public static int Food = Random.Range(5, 10);
        public static int Supplies = Random.Range(10, 15);
    
        public static void StartDay()
        {
            DrunkCustomers = 0;
            switch (CurrentDay)
            {
                case 1:
                    DailyTime = 0;
                    MaxDrunkCustomers = 99;
                    Savings = 100;
                    Rent = 20;
                    Food = Random.Range(5, 10);
                    Supplies = Random.Range(10, 15);
                    break;
                case 2:
                    DailyTime = 240;
                    MaxDrunkCustomers = 99;
                    Rent = 20;
                    Food = Random.Range(5, 10);
                    Supplies = Random.Range(10, 15);
                    break;
                default:
                    DailyTime = 270;
                    MaxDrunkCustomers = 4;
                    Rent = 20;
                    Food = Random.Range(5, 10);
                    Supplies = Random.Range(15, 30);
                    break;
            }
        }

        public static bool IsPosterActive(int posterID)
        {
            foreach (PosterData posterData in Posters)
            {
                if (posterData.hanged != 0 && posterData.id == posterID) return true;
            }
            return false;
        }
        
        public static void UnlockPoster(int posterID)
        {
            PosterData posterData = Posters.Find(p => p.id == posterID);
            if (posterData == null) Debug.LogError("Poster to unlock not found");
            else posterData.visible = true;
        }

        public static void EndDay(float dailyBalance)
        {
            Savings += dailyBalance;
            TodayEarnings = 0;
            CurrentDay++;
            SaveToJSON();
        }
    
        public static void SaveToJSON()
        {
            Save save = new();
            string saveJson = JsonConvert.SerializeObject(save);
            File.WriteAllText(Application.streamingAssetsPath + "/GameData/Save.json", saveJson);
        }
    
        public static void LoadFromJSON()
        {
            string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/GameData/Save.json");
            Save save = JsonConvert.DeserializeObject<Save>(jsonString);
            save.SetGameData();
        }
    }
}