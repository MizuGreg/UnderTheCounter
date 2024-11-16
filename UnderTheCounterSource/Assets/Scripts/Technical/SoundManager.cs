using Hellmade.Sound;
using UnityEngine;

namespace Technical
{
    public class SoundManager : MonoBehaviour
    {
        
        public SoundData soundData;

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

        private void OnLoadMainMenuSound()
        {
            EazySoundManager.PlayMusic(soundData.mainMenuMusic, soundData.backgroundMusicVolume, true, true, 30, 5);
        }

        private void OnLoadBarViewSound()
        {
            EazySoundManager.PlayMusic(soundData.barMusic, soundData.backgroundMusicVolume, true, true, 10, 5);
        }

        private void OnCustomerEnterSound()
        {
            EazySoundManager.PlaySound(soundData.customerEnterSound, soundData.FXVolume);
        }
        
        private void OnCustomerLeaveSound()
        {
            EazySoundManager.PlaySound(soundData.customerLeaveSound, soundData.FXVolume);
        }

        private void OnDayEndSound()
        {
            // todo
        }
        
        // I want to parameterize this instead of having a hundred different functions... is it overkill? probably
    }
}
