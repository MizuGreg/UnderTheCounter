using System.Collections;
using Bar;
using Hellmade.Sound;
using UnityEngine;

namespace Technical
{
    public class SoundManager : MonoBehaviour
    {
        
        public SoundData soundData;
        
        [SerializeField] private float soundEffectCooldownTime = 0.5f;
        [SerializeField] private bool isInCooldown;

        private void Awake()
        {
            EventSystemManager.OnLoadMainMenu += OnLoadMainMenuSound;
            EventSystemManager.OnLoadBarView += OnLoadBarViewSound;
            EventSystemManager.OnLoadShopWindow += OnLoadShopWindowSound;
            EventSystemManager.OnLoadEndOfDay += OnLoadEndOfDaySound;
            EventSystemManager.OnLoadLoseScreen += OnLoadLoseScreenSound;
            
            EventSystemManager.OnRecipeBookOpened += OnRecipeBookOpenedSound;
            EventSystemManager.OnRecipeBookClosed += OnRecipeBookClosedSound;
            EventSystemManager.OnCustomerEnter += OnCustomerEnterSound;
            EventSystemManager.OnCustomerLeave += OnCustomerLeaveSound;
            EventSystemManager.OnDayEnd += OnDayEndSound;
        }

        private void OnDestroy()
        {
            EventSystemManager.OnLoadMainMenu -= OnLoadMainMenuSound;
            EventSystemManager.OnLoadBarView -= OnLoadBarViewSound;
            EventSystemManager.OnRecipeBookOpened -= OnRecipeBookOpenedSound;
            EventSystemManager.OnRecipeBookClosed -= OnRecipeBookClosedSound;
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
            EazySoundManager.PlayMusic(soundData.mainMenuMusic, EazySoundManager.GlobalMusicVolume, true, true, 10, 5);
        }
        
        private void OnLoadShopWindowSound()
        {
            EazySoundManager.PlayMusic(soundData.shopWindowMusic, EazySoundManager.GlobalMusicVolume, true, true, 5, 5);
        }
        
        private void OnLoadEndOfDaySound()
        {
            EazySoundManager.PlayMusic(soundData.endOfDayMusic, EazySoundManager.GlobalMusicVolume, true, true, 5, 2);
        }

        private void OnLoadBarViewSound()
        {
            AudioClip musicClip = soundData.barMusicTracks[Day.CurrentDay-1];
            EazySoundManager.PlayMusic(musicClip, EazySoundManager.GlobalMusicVolume, true, true, 5, 5);
        }

        private void OnLoadLoseScreenSound()
        {
            EazySoundManager.PlayMusic(soundData.loseMusic, EazySoundManager.GlobalMusicVolume, true, true, 2, 5);
        }

        private void OnCustomerEnterSound()
        {
            EazySoundManager.PlaySound(soundData.customerEnterSound, EazySoundManager.GlobalSoundsVolume);
        }
        
        private void OnCustomerLeaveSound()
        {
            EazySoundManager.PlaySound(soundData.customerLeaveSound, EazySoundManager.GlobalSoundsVolume);
        }

        private void OnDayEndSound()
        {
            // todo
        }

        private void OnRecipeBookOpenedSound()
        {
            EazySoundManager.PlaySound(soundData.bookOpenSound, EazySoundManager.GlobalSoundsVolume);
        }

        private void OnRecipeBookClosedSound()
        {
            EazySoundManager.PlaySound(soundData.bookCloseSound, EazySoundManager.GlobalSoundsVolume);
        }
        
        // I want to parameterize this instead of having a hundred different functions... is it overkill? probably
    }
}
