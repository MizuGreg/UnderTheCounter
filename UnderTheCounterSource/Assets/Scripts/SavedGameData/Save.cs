    using System;
    using System.Collections.Generic;
    using ShopWindow;
    using UnityEngine;

    namespace SavedGameData
    {
        [Serializable]
        public class Save
        {
            [SerializeField] private List<Tuple<string, string>> Log;
            [SerializeField] private SerializableDictionary<string, bool> Choices;
        
            [SerializeField] private string BarName;
            [SerializeField] private float DailyTime;
            [SerializeField] private int CurrentDay;
            [SerializeField] private int DrunkCustomers;
            [SerializeField] private int MaxDrunkCustomers;
    
            [SerializeField] private List<PosterData> Posters;
            [SerializeField] private List<int> Trinkets;
    
            [SerializeField] private float Savings;
            [SerializeField] private float TodayEarnings;
            [SerializeField] private int Rent;
            [SerializeField] private int Food;
            [SerializeField] private int Supplies;

            [SerializeField] private DateTime currentDateTime;

            public Save()
            {
                Log = GameData.Log;
                Choices = GameData.Choices;
                BarName = GameData.BarName;
                DailyTime = GameData.DailyTime;
                CurrentDay = GameData.CurrentDay;
                DrunkCustomers = GameData.DrunkCustomers;
                MaxDrunkCustomers = GameData.MaxDrunkCustomers;
                
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
