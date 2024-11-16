using System.Collections.Generic;
using ShopWindow;
using UnityEngine;

namespace Bar
{
    public static class Day
    {
        public static int CurrentDay = 1;
        public static int DrunkCustomers;
        public static int MaxDrunkCustomers = 5; // hardcoded for now
        public static List<PosterManager.Poster> CurrentPosters;
        public static float Savings = 0;
        public static float TodayEarnings = 0;
    }
    
}