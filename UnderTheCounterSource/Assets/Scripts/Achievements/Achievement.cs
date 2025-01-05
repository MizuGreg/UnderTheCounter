using System;
using System.Collections.Generic;
using UnityEngine;

namespace Achievements
{
    [Serializable]
    public class Achievement
    {
        public string id;               // Identifier
        public string title;            // Visible title
        public string description;      
        public bool isUnlocked;         // State of the achievement
        public int progress;            // Actual progress
        public int target;              // Goal to complete
        //public Sprite icon;             // Icon
        
        public override string ToString()
        {
            try
            {
                return $"id: {id}, title: {title}, description: {description}, isUnlocked: {isUnlocked}, progress: {progress}, target: {target}";
            }
            catch (Exception e)
            {
                return $"Error :( Exception in Customer.ToString(): {e}";
            }
        }
    }
    
    [Serializable]
    public class AchievementList
    {
        public List<Achievement> achievements;
    }
}