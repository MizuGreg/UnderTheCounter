using System.Collections.Generic;
using ShopWindow;
using UnityEngine;

namespace Bar
{
    public class DaySO : ScriptableObject
    {
        public static int currentDay = 1;
        public static int drunkCustomers;
        public static int maxDrunkCustomers = 5; // hardcoded for now
        public static List<PosterManager.Poster> currentPosters;
        public static float savings = 0;
        public static float todayEarnings = 0;
    }
    
}