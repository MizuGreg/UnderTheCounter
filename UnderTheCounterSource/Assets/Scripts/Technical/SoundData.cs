using UnityEngine;

namespace Technical
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "Sound Data")]
    public class SoundData : ScriptableObject

    {
        [Header("[Deprecated] Volumes")]
        public float backgroundMusicVolume;
        public float FXVolume;
        
        [Header("Audio Clips/Music")]
        public AudioClip mainMenuMusic;
        public AudioClip shopWindowMusic;
        public AudioClip barMusic;
        public AudioClip blitzMusic;
        public AudioClip daySummaryMusic;
        public AudioClip winMusic;
        public AudioClip loseMusic;
        
        [Header("Audio Clips/Sound Effects")]
        public AudioClip customerEnterSound;
        public AudioClip customerLeaveSound;
        public AudioClip posterHangSound;
        public AudioClip posterTearDownSound;
        public AudioClip sampleSound;

    }
}