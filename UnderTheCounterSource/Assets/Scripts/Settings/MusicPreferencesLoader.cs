using Technical;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class MusicPreferencesLoader : MonoBehaviour
    {
        private SoundManager _soundManager;
        public Slider musicVolumeSlider;
        public Slider sfxVolumeSlider;

        void Start()
        {
            _soundManager = transform.parent.gameObject.GetComponent<SoundManager>();
            
            if (!PlayerPrefs.HasKey("MusicVolume")) PlayerPrefs.SetFloat("MusicVolume", 1);
            if (!PlayerPrefs.HasKey("FXVolume")) PlayerPrefs.SetFloat("FXVolume", 1);

            LoadVolumesOnStartup();
            SetVolumesOnSliders();
        }

        private void LoadVolumesOnStartup()
        {
            SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume"));
            SetFXVolume(PlayerPrefs.GetFloat("FXVolume"));
        }

        private void SetMusicVolume(float volume)
        {
            _soundManager.SetMusicVolume(volume);
        }

        private void SetFXVolume(float volume)
        {
            _soundManager.SetFXVolume(volume);
        }

        private void SetVolumesOnSliders()
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("FXVolume");
        }
    }
}
