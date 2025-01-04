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
        
        private float soundEffectCooldownTime = 0.4f;
        private bool isInCooldown;

        private void Awake()
        {
            EventSystemManager.OnLoadMainMenu += OnLoadMainMenuSound;
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
            EventSystemManager.OnCustomerLeave += OnCustomerLeaveSound;
            EventSystemManager.OnDayEnd += OnDayEndSound;
        }

        private void OnDestroy()
        {
            EventSystemManager.OnLoadMainMenu -= OnLoadMainMenuSound;
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
            EventSystemManager.OnCustomerLeave -= OnCustomerLeaveSound;
            EventSystemManager.OnDayEnd -= OnDayEndSound;
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
        }

        private void OnLoadWinScreenSound()
        {
            EazySoundManager.PlayMusic(soundData.winMusic, 1, true, true, 3, 3);

        }

        private void OnLoadLoseScreenSound()
        {
            EazySoundManager.PlayMusic(soundData.loseMusic, 1, true, true, 1, 3);
        }

        private void OnCustomerEnterSound()
        {
            EazySoundManager.PlaySound(soundData.customerEnterSound, 1);
        }
        
        private void OnCustomerLeaveSound()
        {
            EazySoundManager.PlaySound(soundData.customerLeaveSound, 1);
        }

        private void OnDayEndSound()
        {
            // todo
        }

        private void OnMasterBookOpenedSound()
        {
            EazySoundManager.PlaySound(soundData.bookOpenSound, 1);
        }

        private void OnMasterBookClosedSound()
        {
            EazySoundManager.PlaySound(soundData.bookCloseSound, 1);
        }

        private void OnTabChangedSound()
        {
            EazySoundManager.PlaySound(soundData.tabChangedSound, 0.75f);
        }

        private void OnPageTurnedSound()
        {
            EazySoundManager.PlaySound(soundData.pageTurnedSound, 1);
        }
        
        // I want to parameterize this instead of having a hundred different functions... is it overkill? probably
    }
}
