using System.Collections;
using Bar;
using Hellmade.Sound;
using SavedGameData;
using UnityEngine;

namespace Technical
{
    public class SoundManager : MonoBehaviour
    {
        
        public SoundData soundData;

        private const float soundEffectCooldownTime = 0.3f;
        private bool isInCooldown;

        private void Awake()
        {
            EventSystemManager.OnLoadMainMenu += OnLoadMainMenuSound;
            EventSystemManager.OnLoadMailboxScene += OnLoadMailboxSceneSound;
            EventSystemManager.OnLoadBarView += OnLoadBarViewSound;
            EventSystemManager.OnLoadShopWindow += OnLoadShopWindowSound;
            EventSystemManager.OnLoadEndOfDay += OnLoadEndOfDaySound;
            EventSystemManager.OnLoadWinScreen += OnLoadWinScreenSound;
            EventSystemManager.OnLoadLoseScreen += OnLoadLoseScreenSound;
            
            EventSystemManager.OnMasterBookOpened += OnMasterBookOpenedSound;
            EventSystemManager.OnMasterBookClosed += OnMasterBookClosedSound;
            EventSystemManager.OnTabChanged += OnTabChangedSound;
            EventSystemManager.OnPageTurned += OnPageTurnedSound;
            
            EventSystemManager.OnCustomerEnter += OnCustomerEnterSound;
            EventSystemManager.OnCustomerLeaveSound += OnCustomerLeaveSound;
            EventSystemManager.OnIngredientPouring += OnIngredientPouringSound;
            EventSystemManager.OnTimeWarning += OnTimeWarningSound;
            EventSystemManager.OnTimeUp += OnTimeUpSound;
            EventSystemManager.OnDayEnd += OnDayEndSound;
            
            EventSystemManager.OnBlitzTimerEnded += OnBlitzTimerEndedSound;
            EventSystemManager.OnBlitzCalled += OnBlitzCalledSound;
            EventSystemManager.OnMinigameEnd += OnBlitzWonSound;
            EventSystemManager.OnHowardEnter += OnDoorSlamSound;
            EventSystemManager.OnHowardEnter += OnLoadBarViewSound;
            
            EventSystemManager.OnPosterHung += OnPosterHungSound;
            EventSystemManager.OnPosterRippedDown += OnPosterRippedDownSound;

            EventSystemManager.OnAchievementUnlocked += OnAchievementUnlockedSound;

            EventSystemManager.OnNapoli += OnNapoliSound;
        }

        private void OnDestroy()
        {
            EventSystemManager.OnLoadMainMenu -= OnLoadMainMenuSound;
            EventSystemManager.OnLoadMailboxScene -= OnLoadMailboxSceneSound;
            EventSystemManager.OnLoadBarView -= OnLoadBarViewSound;
            EventSystemManager.OnLoadShopWindow -= OnLoadShopWindowSound;
            EventSystemManager.OnLoadEndOfDay -= OnLoadEndOfDaySound;
            EventSystemManager.OnLoadWinScreen -= OnLoadWinScreenSound;
            EventSystemManager.OnLoadLoseScreen -= OnLoadLoseScreenSound;
            
            EventSystemManager.OnMasterBookOpened -= OnMasterBookOpenedSound;
            EventSystemManager.OnMasterBookClosed -= OnMasterBookClosedSound;
            EventSystemManager.OnTabChanged -= OnTabChangedSound;
            EventSystemManager.OnPageTurned -= OnPageTurnedSound;
            
            EventSystemManager.OnCustomerEnter -= OnCustomerEnterSound;
            EventSystemManager.OnCustomerLeaveSound -= OnCustomerLeaveSound;
            EventSystemManager.OnIngredientPouring -= OnIngredientPouringSound;
            EventSystemManager.OnTimeWarning -= OnTimeWarningSound;
            EventSystemManager.OnTimeUp -= OnTimeUpSound;
            EventSystemManager.OnDayEnd -= OnDayEndSound;

            EventSystemManager.OnBlitzTimerEnded -= OnBlitzTimerEndedSound;
            EventSystemManager.OnBlitzCalled -= OnBlitzCalledSound;
            EventSystemManager.OnMinigameEnd -= OnBlitzWonSound;
            EventSystemManager.OnHowardEnter -= OnDoorSlamSound;
            EventSystemManager.OnHowardEnter -= OnLoadBarViewSound;
            
            EventSystemManager.OnPosterHung -= OnPosterHungSound;
            EventSystemManager.OnPosterRippedDown -= OnPosterRippedDownSound;
            
            EventSystemManager.OnAchievementUnlocked -= OnAchievementUnlockedSound;
            
            EventSystemManager.OnNapoli -= OnNapoliSound;
        }

        public void SetMusicVolume(float volume)
        {
            EazySoundManager.GlobalMusicVolume = volume;
            PlayerPrefs.SetFloat("MusicVolume", volume);
        }

        public void SetFXVolume(float volume)
        {
            EazySoundManager.GlobalSoundsVolume = volume;
            PlayerPrefs.SetFloat("FXVolume", volume);
        }

        public void PlaySampleSound()
        {
            if (!isInCooldown)
            {
                EazySoundManager.PlaySound(soundData.sampleSound, EazySoundManager.GlobalSoundsVolume);
                StartCoroutine(Cooldown());
            }
        }

        private IEnumerator Cooldown()
        {
            isInCooldown = true;
            yield return new WaitForSeconds(soundEffectCooldownTime);
            isInCooldown = false;
        }

        private void OnLoadMainMenuSound()
        {

            EazySoundManager.PlayMusic(soundData.mainMenuMusic, 1, true, true, 5, 3);

        }

        private void OnLoadMailboxSceneSound()
        {
            EazySoundManager.PlayMusic(soundData.mailboxMusic, 1, true, true, 5, 5);
        }
        
        private void OnLoadShopWindowSound()
        {
            // EazySoundManager.PlayMusic(soundData.shopWindowMusic, 1, true, true, 5, 3);
        }
        
        private void OnLoadEndOfDaySound()
        {
            EazySoundManager.PlayMusic(soundData.endOfDayMusic, 1, true, true, 3, 1);
        }

        private void OnLoadBarViewSound()
        {
            AudioClip musicClip = soundData.barMusicTracks[GameData.CurrentDay-1];
            EazySoundManager.PlayMusic(musicClip, 1, true, true, 5, 3);
            print($"Day {GameData.CurrentDay}. Now playing: {soundData.barMusicTracks[GameData.CurrentDay-1].name}.");
        }

        private void OnLoadWinScreenSound()
        {
            EazySoundManager.PlayMusic(soundData.winMusic, 1, true, true, 3, 3);

        }

        private void OnLoadLoseScreenSound(string endingType)
        {
            EazySoundManager.PlayMusic(soundData.loseMusic, 1, true, true, 1, 3);
        }

        private void OnCustomerEnterSound()
        {
            EazySoundManager.PlaySound(soundData.customerEnterSound);
        }
        
        private void OnCustomerLeaveSound()
        {
            EazySoundManager.PlaySound(soundData.customerLeaveSound);
        }

        private void OnIngredientPouringSound()
        {
            // Commented out, sound is too annoying when repeated several times...
            // EazySoundManager.PlaySound(soundData.ingredientPouredSound);
        }

        private void OnTimeWarningSound()
        {
            EazySoundManager.PlaySound(soundData.timeWarningSound, 0.70f);
        }
        
        private void OnTimeUpSound()
        {
            EazySoundManager.PlaySound(soundData.timeUpSound);
        }
        
        private void OnDayEndSound()
        {
            // nothing... for now
        }

        private void OnMasterBookOpenedSound()
        {
            EazySoundManager.PlaySound(soundData.bookOpenSound, 0.8f);
        }

        private void OnMasterBookClosedSound()
        {
            EazySoundManager.PlaySound(soundData.bookCloseSound, 0.55f);
        }

        private void OnTabChangedSound()
        {
            EazySoundManager.PlaySound(soundData.tabChangedSound, 0.55f);
        }

        private void OnPageTurnedSound()
        {
            EazySoundManager.PlaySound(soundData.pageTurnedSound);
        }

        private void OnBlitzTimerEndedSound()
        {
            EazySoundManager.PlaySound(soundData.blitzTimerEndSound);
        }

        private void OnPosterHungSound()
        {
            EazySoundManager.PlaySound(soundData.posterHungSound);
        }

        private void OnPosterRippedDownSound()
        {
            EazySoundManager.PlaySound(soundData.posterTearDownSound);
        }

        private void OnBlitzCalledSound()
        {
            StartCoroutine(PlayBlitzMusic());
        }

        private IEnumerator PlayBlitzMusic()
        {
            yield return new WaitForSeconds(1.0f);
            EazySoundManager.PlayMusic(soundData.blitzMusic, 0.8f, true, true, 3, 3);
        }

        private void OnDoorSlamSound()
        {
            StartCoroutine(PlayDoorSlamSound());
        }

        private IEnumerator PlayDoorSlamSound()
        {
            yield return new WaitForSeconds(0.75f);
            EazySoundManager.PlaySound(soundData.doorSlamSound, 0.9f);
        }

        private void OnBlitzWonSound()
        {
            EazySoundManager.PlaySound(soundData.blitzWonSound, 0.3f);
        }

        private void OnAchievementUnlockedSound()
        {
            EazySoundManager.PlaySound(soundData.achievementSound);
        }

        private void OnNapoliSound()
        {
            EazySoundManager.PlaySound(soundData.napoliSound);
        }
    }
}
