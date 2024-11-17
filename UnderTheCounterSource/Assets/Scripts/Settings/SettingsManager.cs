using Bar;
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
            fullscreenToggle.isOn = Screen.fullScreen;
            
            if (!PlayerPrefs.HasKey("MusicVolume")) PlayerPrefs.SetFloat("MusicVolume", 1);
            if (!PlayerPrefs.HasKey("FXVolume")) PlayerPrefs.SetFloat("FXVolume", 1);
            if (!PlayerPrefs.HasKey("TextSpeed")) PlayerPrefs.SetFloat("TextSpeed", 20);

            LoadVolumesOnStartup();
            LoadTextSpeedOnStartup();
        }

        private void LoadVolumesOnStartup()
        {
            float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            float fxVolume = PlayerPrefs.GetFloat("FXVolume");
            
            
            SetMusicVolume(musicVolume);
            musicVolumeSlider.value = musicVolume;
            
            SetFXVolume(fxVolume);
            sfxVolumeSlider.value = fxVolume;
        }

        private void LoadTextSpeedOnStartup()
        {
            float textSpeed = PlayerPrefs.GetFloat("TextSpeed");
            dialogueManager.SetNormalTextSpeed(textSpeed);
            textSpeedSlider.value = textSpeed;
        }

        public void SetMusicVolume(float volume) 
        {
            soundManager.SetMusicVolume(volume);
        }

        public void SetFXVolume(float volume) 
        {
            soundManager.SetFXVolume(volume);
        }

        public void ToggleFullScreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
            fullscreenToggle.isOn = Screen.fullScreen;
            Debug.Log(Screen.fullScreen ? "Screen is set to fullscreen" : "Screen is windowed");
        }

        public void SetTextSpeed(float speed)
        {
            dialogueManager.SetNormalTextSpeed(speed);
            PlayerPrefs.SetFloat("TextSpeed", speed);
        }
    }
}
