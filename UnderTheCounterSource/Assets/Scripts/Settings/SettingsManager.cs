using Bar;
using Hellmade.Sound;
using UnityEngine;
using UnityEngine.UI;
using Technical;

namespace Settings
{
    public class SettingsManager : MonoBehaviour 
    {
        [Header("Sound")]
        [SerializeField] private SoundManager soundManager;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        
        [Header("Dialogue")]
        [SerializeField] private DialogueManager dialogueManager;
        [SerializeField] private Slider textSpeedSlider;
        
        [Header("Other")]
        [SerializeField] private Toggle fullscreenToggle;

        void Start()
        {
            if (!PlayerPrefs.HasKey("MusicVolume")) PlayerPrefs.SetFloat("MusicVolume", 1);
            if (!PlayerPrefs.HasKey("FXVolume")) PlayerPrefs.SetFloat("FXVolume", 1);
            if (!PlayerPrefs.HasKey("TextSpeed")) PlayerPrefs.SetFloat("TextSpeed", 20);
            if (!PlayerPrefs.HasKey("Fullscreen")) PlayerPrefs.SetInt("Fullscreen", 1);

            LoadVolumesOnStartup();
            LoadTextSpeedOnStartup();
            LoadFullscreenOnStartup();
        }

        private void LoadVolumesOnStartup()
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            float fxVolume = PlayerPrefs.GetFloat("FXVolume");
            
            
            // SetMusicVolume(musicVolume);
            musicVolumeSlider.value = musicVolume;
            
            // SetFXVolume(fxVolume);
            sfxVolumeSlider.value = fxVolume;
        }

        private void LoadTextSpeedOnStartup()
        {
            float textSpeed = PlayerPrefs.GetFloat("TextSpeed");
            if (dialogueManager != null) dialogueManager.SetNormalTextSpeed(textSpeed);
            textSpeedSlider.value = textSpeed;
        }

        private void LoadFullscreenOnStartup()
        {
            fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1;
            ToggleFullScreen();
        }

        public void SetMusicVolume(float volume) 
        {
            soundManager.SetMusicVolume(volume);
        }

        public void SetFXVolume(float volume) 
        {
            soundManager.SetFXVolume(volume);
            if (sfxVolumeSlider.gameObject.activeInHierarchy) soundManager.PlaySampleSound();
        }

        public void ToggleFullScreen()
        {
            Screen.fullScreen = fullscreenToggle.isOn;
            PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        }

        public void SetTextSpeed(float speed)
        {
            if (dialogueManager != null) dialogueManager.SetNormalTextSpeed(speed);
            PlayerPrefs.SetFloat("TextSpeed", speed);
        }

        public void ReduceMusicWhenUnfocused()
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            float fxVolume = PlayerPrefs.GetFloat("FXVolume");
            
            SetMusicVolume(musicVolume/2);
            SetFXVolume(fxVolume/2);
        }

        public void RevertMusicWhenUnfocused()
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            float fxVolume = PlayerPrefs.GetFloat("FXVolume");
            
            SetMusicVolume(musicVolume);
            SetFXVolume(fxVolume);
        }
        
    }
}
