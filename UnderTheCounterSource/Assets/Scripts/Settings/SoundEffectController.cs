using UnityEngine;
using UnityEngine.Audio;

namespace Settings
{
    public class SoundEffectController : MonoBehaviour
    {
        public AudioSource audioSource;

        void Start()
        {
            audioSource.loop = false;
            audioSource.volume = PlayerPrefs.GetFloat("SoundEffectsVolume", 1);
        }

        public void PlaySound(AudioClip clip)
        {
            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
        }

        public void SetVolume(float volume)
        {
            Mixer.SetFloat("SoundEffectsVolume", Mathf.Log10(volume)*20); // or maybe it should be just "Volume"? idk
            audioSource.volume = volume;
        }
    }
}
