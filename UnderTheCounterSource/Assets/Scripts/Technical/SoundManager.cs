using System.Collections;
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
            
            EventSystemManager.OnCustomerEnter += OnCustomerEnterSound;
            EventSystemManager.OnCustomerLeave += OnCustomerLeaveSound;
            EventSystemManager.OnDayEnd += OnDayEndSound;
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

        private void OnLoadBarViewSound()
        {
            EazySoundManager.PlayMusic(soundData.barMusic, EazySoundManager.GlobalMusicVolume, true, true, 10, 5);
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
        
        // I want to parameterize this instead of having a hundred different functions... is it overkill? probably
    }
}
