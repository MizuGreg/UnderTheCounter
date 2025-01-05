using System;
using System.Collections.Generic;
using System.IO;
using AYellowpaper.SerializedCollections;
using Newtonsoft.Json;
using ShopWindow;
using UnityEngine;

namespace SavedGameData
{
    public static class GameData
    {
        public static List<Tuple<string, string>> Log = new();
        public static SerializedDictionary<string, bool> Choices = new();
        
        public static string BarName = "The Chitchat";
        public static float DailyTime = 240;
        public static int CurrentDay = 1;
        public static int DrunkCustomers = 0;
        public static int MaxDrunkCustomers = 99;
    
        public static List<Poster> Posters = new();
        public static List<int> Trinkets = new();
    
        public static float Savings = 100;
        public static float TodayEarnings = 0;
        public static int Rent = 10;
        public static int Food = 10;
        public static int Supplies = 10;

        public static void Initialize()
        {
            Log = new();
            Choices = new()
            {
                ["MargaretDrunk"] = false,
                ["MafiaDeal"] = false
            };

            BarName = "The Chitchat";
            DailyTime = 240;
            CurrentDay = 1;
            DrunkCustomers = 0;
            MaxDrunkCustomers = 99;
        
            Posters = new();
            Trinkets = new();
        
            Savings = 100;
            TodayEarnings = 0;
            Rent = 10;
            Food = 10;
            Supplies = 10;
        }
        
        public static void StartDay()
        {
            DrunkCustomers = 0;
            switch (CurrentDay)
            {
                case 1:
                    DailyTime = 0;
                    MaxDrunkCustomers = 99;
                    Savings = 100;
                    Rent = 10;
                    Food = 10;
                    Supplies = 10;
                    break;
                case 2:
                    DailyTime = 240;
                    MaxDrunkCustomers = 99;
                    Rent = 10;
                    Food = 10;
                    Supplies = 10;
                    break;
                case 3:
                    DailyTime = 270;
                    MaxDrunkCustomers = 4;
                    Rent = 10;
                    Food = 10;
                    Supplies = 20;
                    break;
                case 4:
                    DailyTime = 270;
                    MaxDrunkCustomers = 2; // for testing purposes
                    Rent = 10;
                    Food = 10;
                    Supplies = 30;
                    break;
                case 5:
                    DailyTime = 270;
                    MaxDrunkCustomers = 4;
                    Rent = 10;
                    Food = 10;
                    Supplies = 40;
                    break;
                case 6:
                    DailyTime = 270;
                    MaxDrunkCustomers = 4;
                    Rent = 10;
                    Food = 10;
                    Supplies = 50;
                    break;
                case 7: default:
                    DailyTime = 270;
                    MaxDrunkCustomers = 4;
                    Rent = 10;
                    Food = 10;
                    Supplies = 50;
                    break;
            }
        }

        public static bool IsPosterActive(int posterID)
        {
            foreach (Poster poster in Posters)
            {
                if (poster.hanged != 0 && poster.id == posterID) return true;
            }
            return false;
        }
        
        public static void UnlockPoster(int posterID)
        {
            Poster poster = Posters.Find(p => p.id == posterID);
            if (poster == null) Debug.LogError("Poster to unlock not found");
            else poster.visible = true;
        }

        public static void EndDay(float dailyBalance)
        {
            Savings += dailyBalance;
            TodayEarnings = 0;
            CurrentDay++;
            SaveToJson();
        }
    
        public static void SaveToJson()
        {
            Save save = new(); // creates a snapshot of GameData
            string saveJson = JsonConvert.SerializeObject(save, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // Sprites have a self-referencing variable so this ignores them
            });
            File.WriteAllText(Application.streamingAssetsPath + "/GameData/Save.json", saveJson);
            Debug.Log("Saved game data.");
        }
    
        public static void LoadFromJson()
        {
            Debug.Log("Loading game data.");
            try
            {
                string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/GameData/Save.json");
                Save save = JsonConvert.DeserializeObject<Save>(jsonString);
                save.SetGameData();
            }
            catch (Exception e)
            {
                Debug.Log("Error while loading game data. Will default to a new game. Error below.");
                Debug.LogError(e);
                
                Initialize();
            }
            
        }
    }
}