using UnityEngine;
using UnityEngine.UI;
using Hellmade.Sound;
using Technical;

namespace Settings
{
    public class SettingsManager : MonoBehaviour 
    {
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider soundEffectsVolumeSlider;
        [SerializeField] private SoundManager soundManager;
        [SerializeField] private Toggle fullscreenToggle;

        void Start() 
        {
            if (!PlayerPrefs.HasKey("MusicVolume")) 
            {
                PlayerPrefs.SetFloat("MusicVolume", 1);
            }
            if (!PlayerPrefs.HasKey("FXVolume")) 
            {
                PlayerPrefs.SetFloat("FXVolume", 1);
            }

            LoadVolumesOnStartup();
            SetVolumesOnSliders();
        }

        public void SetMusicVolume(float volume) 
        {
            soundManager.SetMusicVolume(volume);
        }

        public void SetFXVolume(float volume) 
        {
            soundManager.SetFXVolume(volume);
        }

        private void LoadVolumesOnStartup() 
        {
            SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume"));
            SetFXVolume(PlayerPrefs.GetFloat("FXVolume"));
        }

        private void SetVolumesOnSliders()
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            soundEffectsVolumeSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume");
        }

        public void ToggleFullScreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
            fullscreenToggle.isOn = Screen.fullScreen;
            Debug.Log(Screen.fullScreen ? "Screen is set to fullscreen" : "Screen is windowed");
        }
    }
}
