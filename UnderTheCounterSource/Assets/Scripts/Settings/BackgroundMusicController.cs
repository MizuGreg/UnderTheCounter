using UnityEngine;

namespace Settings
{
    public class BackgroundMusicController : MonoBehaviour
    {
        public AudioSource audioSource;

        void Start()
        {
            audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
            PlayMusic();
        }

        public void PlayMusic()
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        public void SetVolume(float volume)
        {
            audioSource.volume = volume;
        }
    }
}
