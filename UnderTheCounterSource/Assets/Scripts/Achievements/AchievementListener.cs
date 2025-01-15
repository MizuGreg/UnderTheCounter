using System;
using Technical;
using UnityEngine;

namespace Achievements
{
    public class AchievementListener : MonoBehaviour
    {
        
        void Awake()
        {
            // Subscribe to events
            EventSystemManager.OnTutorialCompleted += TutorialCompleted;
            EventSystemManager.OnDealMade += DealWithTheDevil;
            EventSystemManager.OnHalfTrinketCollected += AspiringCollector;
            EventSystemManager.OnBankrupt += Bankrupt;
            EventSystemManager.OnBlitzLose += BlitzedOut;
            EventSystemManager.OnWin += EndOfTheLine;
            EventSystemManager.OnButterfly1 += EveryCloud;
            EventSystemManager.OnButterfly2 += HasASilverLining;
            EventSystemManager.OnBarBurned += MafiasPawn;
            EventSystemManager.OnDealRefused += PlayingItClean;
            EventSystemManager.OnCocktailWatered += ShakenStirredStretched;
            EventSystemManager.OnBackstabbed += TheBackstabber;
            EventSystemManager.OnAllTrinketCollected += UltimateCollector;
            EventSystemManager.OnAllCustomersServed += BeenServed;
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            EventSystemManager.OnTutorialCompleted -= TutorialCompleted;
            EventSystemManager.OnDealMade -= DealWithTheDevil;
            EventSystemManager.OnHalfTrinketCollected -= AspiringCollector;
            EventSystemManager.OnBankrupt -= Bankrupt;
            EventSystemManager.OnBlitzLose -= BlitzedOut;
            EventSystemManager.OnWin -= EndOfTheLine;
            EventSystemManager.OnButterfly1 -= EveryCloud;
            EventSystemManager.OnButterfly2 -= HasASilverLining;
            EventSystemManager.OnBarBurned -= MafiasPawn;
            EventSystemManager.OnDealRefused -= PlayingItClean;
            EventSystemManager.OnCocktailWatered -= ShakenStirredStretched;
            EventSystemManager.OnBackstabbed -= TheBackstabber;
            EventSystemManager.OnAllTrinketCollected -= UltimateCollector; 
            EventSystemManager.OnAllCustomersServed -= BeenServed;
        }

        private void TutorialCompleted()
        {
            EventSystemManager.OnAchievementProgress("tutorial completed", 1);
        }

        public void NewspaperOpened()
        {
            EventSystemManager.OnAchievementProgress("newspaper opened", 1);
        }
        
        private void DealWithTheDevil()
        {
            EventSystemManager.OnAchievementProgress("deal with the devil", 1);
        }

        private void AspiringCollector()
        {
            EventSystemManager.OnAchievementProgress("aspiring collector", 1);
        }

        private void Bankrupt()
        {
            EventSystemManager.OnAchievementProgress("bankrupt", 1);
        }

        private void BlitzedOut()
        {
            EventSystemManager.OnAchievementProgress("blitzed out", 1);
        }

        private void EndOfTheLine()
        {
            EventSystemManager.OnAchievementProgress("end of the line", 1);
        }

        private void EveryCloud()
        {
            EventSystemManager.OnAchievementProgress("every cloud", 1);
        }

        private void HasASilverLining()
        {
            EventSystemManager.OnAchievementProgress("has a silver lining", 1);
        }

        private void MafiasPawn()
        {
            EventSystemManager.OnAchievementProgress("mafia's pawn", 1);
        }

        private void PlayingItClean()
        {
            EventSystemManager.OnAchievementProgress("playing it clean", 1);
        }

        private void ShakenStirredStretched()
        {
            EventSystemManager.OnAchievementProgress("shaken, stirred and stretched", 1);
        }

        private void TheBackstabber()
        {
            EventSystemManager.OnAchievementProgress("the backstabber", 1);
        }

        private void UltimateCollector()
        {
            EventSystemManager.OnAchievementProgress("ultimate collector", 1);
        }

        private void BeenServed()
        {
            EventSystemManager.OnAchievementProgress("you've been served", 1);
        }
        
    }
}
