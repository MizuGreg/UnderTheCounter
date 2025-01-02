using System;
using Technical;
using UnityEngine;

namespace Achievements
{
    public class AchievementListener : MonoBehaviour
    {
        
        void Start()
        {
            // Subscribe to events
            EventSystemManager.OnTutorial1End += TutorialCompleted;
            EventSystemManager.OnGarnishAdded += CocktailServed;
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            EventSystemManager.OnTutorial1End -= TutorialCompleted;
            EventSystemManager.OnGarnishAdded -= CocktailServed;
        }

        private void TutorialCompleted()
        {
            EventSystemManager.OnAchievementProgress("tutorial completed", 1);
        }

        private void CocktailServed()
        {
            EventSystemManager.OnAchievementProgress("10 cocktails", 1);
            EventSystemManager.OnAchievementProgress("50 cocktails", 1);
        }

        public void NewspaperOpened()
        {
            EventSystemManager.OnAchievementProgress("newspaper opened", 1);
        }
    }
}
