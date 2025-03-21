using System;
using System.Collections.Generic;
using System.IO;
using AYellowpaper.SerializedCollections;
using Newtonsoft.Json;
using ShopWindow;
using UnityEngine;
using Technical;

namespace SavedGameData
{
    public static class GameData
    {
        public static readonly string SaveFilePath = Application.streamingAssetsPath + "/GameData/Save.json"; // obsolete
        
        public static List<Tuple<string, string>> Log = new();
        public static SerializedDictionary<string, bool> Choices = new(){
            ["MargaretDrunk"] = false,
            ["MafiaDeal"] = false,
            ["PayoffAccepted"] = false,
            ["UnionSnitched"] = false
        };
        
        public static string BarName = "The Chitchat";
        public static float DailyTime = 240;
        public static int CurrentDay = 1;
        
        public static int DrunkCustomers = 0;
        public static int MaxDrunkCustomers = 99;
        public static float BlitzTime = 12;
        public static bool HasABlitzHappened = false;
        public static int BlitzFailCounter = 0;
        public static bool WasLastBlitzFailed = false;
        public static bool fastDay = false;
        public static bool allCustomersServed = false;
        public static string loseType;
    
        public static List<Poster> Posters = new();
        public static List<int> Trinkets = new();

        public static int payoffAmount = 50;
    
        public static float Savings = 50;
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
                ["MafiaDeal"] = false,
                ["PayoffAccepted"] = false,
                ["UnionSnitched"] = false
            };

            BarName = "The Chitchat";
            DailyTime = 240;
            CurrentDay = 1;
            
            DrunkCustomers = 0;
            MaxDrunkCustomers = 99;
            BlitzTime = 12;
            BlitzFailCounter = 0;
            HasABlitzHappened = false;
            WasLastBlitzFailed = false;
            fastDay = false;
        
            Posters = new();
            Trinkets = new();
        
            Savings = 50;
            TodayEarnings = 0;
            Rent = 10;
            Food = 10;
            Supplies = 10;
        }
        
        public static void StartDay()
        {
            DrunkCustomers = 0;
            fastDay = false;
            switch (CurrentDay)
            {
                case 1:
                    DailyTime = 0;
                    Savings = 50;
                    Rent = 10;
                    Food = 10;
                    Supplies = 10;
                    break;
                case 2:
                    DailyTime = 240;
                    Rent = 10;
                    Food = 10;
                    Supplies = 10;
                    break;
                case 3:
                    DailyTime = 270;
                    Rent = 10;
                    Food = 10;
                    Supplies = 20;
                    break;
                case 4:
                    DailyTime = 270;
                    Rent = 10;
                    Food = 10;
                    Supplies = 30;
                    break;
                case 5:
                    DailyTime = 270;
                    Rent = 10;
                    Food = 10;
                    Supplies = 35;
                    break;
                case 6:
                    DailyTime = 270;
                    Rent = 10;
                    Food = 10;
                    Supplies = 40;
                    break;
                case 7: default:
                    DailyTime = 270;
                    Rent = 10;
                    Food = 10;
                    Supplies = 40;
                    break;
            }
            WasLastBlitzFailed = false;
            UpdateBlitzVariables();
            PosterEffects();
        }

        private static void PosterEffects()
        {
            if (IsPosterActive(4)) Supplies += 10; // increases supplies cost if baroque poster is active
            if (IsPosterActive(6)) BlitzTime += 2;
        }

        public static void BlitzSuccessful()
        {
            HasABlitzHappened = true;
            WasLastBlitzFailed = false;
            if (BlitzFailCounter > 0) BlitzFailCounter--;
            UpdateBlitzVariables();
        }

        public static void BlitzFailed()
        {
            HasABlitzHappened = true;
            WasLastBlitzFailed = true;
            BlitzFailCounter++;
            // trigger popup blink
            EventSystemManager.OnWrongChoice();
            UpdateBlitzVariables();
        }
        
        private static void UpdateBlitzVariables()
        {
            if (CurrentDay < 3)
            {
                BlitzTime = 99;
                MaxDrunkCustomers = 99;
                return;
            }

            BlitzTime = (HasABlitzHappened ? 9 : 12) // blitz lasts more if it's the first blitz ever
                        - 2 * BlitzFailCounter; // reduce proportionately to how many blitzes you've failed "lately"
            MaxDrunkCustomers = 4 - (WasLastBlitzFailed ? 1 : 0); // reduce threshold by 1 if last blitz was failed
            if (IsPosterActive(4)) MaxDrunkCustomers++; // increase threshold if poster with id 4 is hung
        }

        public static bool IsPosterActive(int posterID)
        {
            foreach (Poster poster in Posters)
            {
                if (poster.hanged != 0 && poster.id == posterID) return true;
            }
            return false;
        }
        
        public static bool HasHungPosters()
        {
            foreach (Poster poster in Posters)
            {
                if (poster.hanged != 0) return true;
            }
            return false;
        }
        
        public static void UnlockPoster(int posterID)
        {
            Debug.Log($"poster {posterID} unlocked");
            Poster poster = Posters.Find(p => p.id == posterID);
            if (poster == null) Debug.LogError("Poster to unlock not found");
            else poster.visible = true;
        }

        public static void EndDay(float dailyBalance)
        {
            Savings += dailyBalance;
            TodayEarnings = 0;
            Log.Clear();
        }
    
        public static void SaveToJson()
        {
            Save save = new(); // creates a snapshot of GameData
            string saveJson = JsonConvert.SerializeObject(save, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore // Sprites have a self-referencing variable so this ignores them
            });
            PlayerPrefs.SetString("Save", saveJson);
            Debug.Log("Saved game data.");
        }
    
        public static void LoadFromJson()
        {
            Debug.Log("Loading game data.");
            try
            {
                string jsonString = PlayerPrefs.GetString("Save");
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

        public static void DeleteSave()
        {
            Debug.Log("Deleting save file.");
            try
            {
                PlayerPrefs.DeleteKey("Save");
            }
            catch (Exception e)
            {
                Debug.Log("Error while deleting save file:");
                Debug.LogError(e);
            }
        }
    }
}