using UnityEngine;
using UnityEngine.UI;
using Hellmade.Sound;

namespace Settings
{
    public class SettingsManager : MonoBehaviour 
    {
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider soundEffectsVolumeSlider;
        [SerializeField] private BackgroundMusicController backgroundMusicController;
        [SerializeField] private SoundEffectController soundEffectController;

        void Start() 
        {
            if (!PlayerPrefs.HasKey("MusicVolume")) 
            {
                PlayerPrefs.SetFloat("MusicVolume", 1);
            }
            if (!PlayerPrefs.HasKey("SoundEffectsVolume")) 
            {
                PlayerPrefs.SetFloat("SoundEffectsVolume", 1);
            }

            LoadVolumes();
        }

        public void SetMusicVolume(float volume) 
        {
            backgroundMusicController.SetVolume(musicVolumeSlider.value); // Chiama SetVolume
            SaveMusicVolume();
        }

        public void SetSoundEffectsVolume(float volume) 
        {
            soundEffectController.SetVolume(soundEffectsVolumeSlider.value); 
            SaveSoundEffectsVolume();
        }

        private void SaveMusicVolume() 
        {
            PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        }

        private void SaveSoundEffectsVolume() 
        {
            PlayerPrefs.SetFloat("SoundEffectsVolume", soundEffectsVolumeSlider.value);
        }

        private void LoadVolumes() 
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            soundEffectsVolumeSlider.value = PlayerPrefs.GetFloat("SoundEffectsVolume");

            backgroundMusicController.SetVolume(musicVolumeSlider.value);
            soundEffectController.SetVolume(soundEffectsVolumeSlider.value);
        }

        public void ToggleFullScreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
            Debug.Log(Screen.fullScreen ? "Screen is set to fullscreen" : "Screen is windowed");
        }
    }
}
