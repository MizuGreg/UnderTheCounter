using UnityEngine;
using UnityEngine.UI;
using Technical;

namespace Settings
{
    public class SettingsManager : MonoBehaviour 
    {
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private SoundManager soundManager;
        [SerializeField] private Toggle fullscreenToggle;

        public void Start()
        {
            fullscreenToggle.isOn = Screen.fullScreen;
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
    }
}
