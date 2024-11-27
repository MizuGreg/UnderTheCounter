using System.Collections.Generic;
using ShopWindow;
using UnityEngine;

namespace Bar
{
    public static class Day
    {
        public static float DailyTime = 120;
        public static int CurrentDay = 1;
        public static int DrunkCustomers;
        public static int MaxDrunkCustomers = 5;
        public static List<ShopWindowManager.Poster> CurrentPosters;
        public static float Savings = 0;
        public static float TodayEarnings = 0;

        public static void Initialize()
        {
            CurrentDay = 1;
            DrunkCustomers = 0;
            CurrentPosters = new();
            Savings = 0;
            TodayEarnings = 0;
        }
    }
    
}