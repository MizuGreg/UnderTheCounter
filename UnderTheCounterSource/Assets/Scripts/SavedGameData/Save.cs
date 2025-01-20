    using System;
    using System.Collections.Generic;
    using AYellowpaper.SerializedCollections;
    using Newtonsoft.Json;
    using ShopWindow;

    namespace SavedGameData
    {
        [JsonObject(MemberSerialization.Fields)]
        public class Save
        {
            private List<Tuple<string, string>> Log;
            private SerializedDictionary<string, bool> Choices;
        
            private string BarName;
            private float DailyTime;
            private int CurrentDay;
        
            private int DrunkCustomers;
            private int MaxDrunkCustomers;
            private float BlitzTime;
            private bool HasABlitzHappened;
            private int BlitzFailCounter;
            private bool WasLastBlitzFailed;
            private bool fastDay;
            private bool allCustomersServed;
            private string loseType;
    
            private List<Poster> Posters;
            private List<int> Trinkets;

            private int payoffAmount;
    
            private float Savings;
            private float TodayEarnings;
            private int Rent;
            private int Food;
            private int Supplies;

            private DateTime currentDateTime;

            public Save()
            {
                Log = GameData.Log;
                Choices = GameData.Choices;
                
                BarName = GameData.BarName;
                DailyTime = GameData.DailyTime;
                CurrentDay = GameData.CurrentDay;
                
                DrunkCustomers = GameData.DrunkCustomers;
                MaxDrunkCustomers = GameData.MaxDrunkCustomers;
                BlitzTime = GameData.BlitzTime;
                HasABlitzHappened = GameData.HasABlitzHappened;
                BlitzFailCounter = GameData.BlitzFailCounter;
                fastDay = GameData.fastDay;
                allCustomersServed = GameData.allCustomersServed;
                loseType = GameData.loseType;
                
                Posters = GameData.Posters;
                Trinkets = GameData.Trinkets;
                
                Savings = GameData.Savings;
                TodayEarnings = GameData.TodayEarnings;
                Rent = GameData.Rent;
                Food = GameData.Food;
                Supplies = GameData.Supplies;
                
                currentDateTime = DateTime.Now;
            }

            public void SetGameData()
            {
                GameData.Log = Log;
                GameData.Choices = Choices;
                GameData.BarName = BarName;
                GameData.DailyTime = DailyTime;
                GameData.CurrentDay = CurrentDay;
                GameData.DrunkCustomers = DrunkCustomers;
                GameData.MaxDrunkCustomers = MaxDrunkCustomers;
                
                GameData.Posters = Posters;
                GameData.Trinkets = Trinkets;
                
                GameData.Savings = Savings;
                GameData.TodayEarnings = TodayEarnings;
                GameData.Rent = Rent;
                GameData.Food = Food;
                GameData.Supplies = Supplies;
            }
        }
    }
