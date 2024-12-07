using System.Collections.Generic;
using ShopWindow;
using UnityEngine;

namespace Bar
{
    public static class Day
    {
        public static float DailyTime = 180;
        public static int CurrentDay = 1;
        public static int DrunkCustomers;
        public static int MaxDrunkCustomers = 3;
        public static List<Poster> CurrentPosters = new();
        public static float Savings = 0;
        public static float TodayEarnings = 0;

        public static void Initialize()
        {
            CurrentDay = 1;
            DrunkCustomers = 0;
            CurrentPosters = new();
            Savings = 50;
            TodayEarnings = 0;
        }

        public static void StartDay()
        {
            DrunkCustomers = 0;
            switch (CurrentDay)
            {
                case 1:
                    DailyTime = 0;
                    MaxDrunkCustomers = 99;
                    break;
                case 2:
                    DailyTime = 180;
                    MaxDrunkCustomers = 99;
                    break;
                default:
                    DailyTime = 200;
                    MaxDrunkCustomers = 3;
                    break;
            }
        }

        public static bool IsPosterActive(int posterID)
        {
            foreach (Poster poster in CurrentPosters)
            {
                if (poster.posterID == posterID) return true;
            }
            return false;
        }

        public static void EndDay(float dailyBalance)
        {
            Savings += dailyBalance;
            TodayEarnings = 0;
            CurrentDay++;
            CurrentPosters.Clear(); // Removes all posters/effects
        }
    }
    
}