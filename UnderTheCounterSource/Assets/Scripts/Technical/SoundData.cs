using UnityEngine;
using UnityEngine.Serialization;

namespace Technical
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "Sound Data")]
    public class SoundData : ScriptableObject

    {
        [Header("[Deprecated] Volumes")]
        public float backgroundMusicVolume;
        public float FXVolume;
        
        [Header("Music")]
        public AudioClip mainMenuMusic;
        public AudioClip shopWindowMusic;
        public AudioClip[] barMusicTracks;
        public AudioClip blitzMusic;
        public AudioClip endOfDayMusic;
        public AudioClip winMusic;
        public AudioClip loseMusic;
        
        [Header("Sound effects")]
        [Header("Technical")]
        public AudioClip sampleSound;
        
        [Header("Bar")]
        public AudioClip customerEnterSound;
        public AudioClip customerLeaveSound;
        public AudioClip timeWarningSound;
        public AudioClip timeUpSound;
        public AudioClip ingredientPouredSound;
        
        [Header("Shop window")]
        public AudioClip posterHungSound;
        public AudioClip posterTearDownSound;
        
        [Header("Master book")]
        public AudioClip bookOpenSound;
        public AudioClip bookCloseSound;
        public AudioClip tabChangedSound;
        public AudioClip pageTurnedSound;
        
        [Header("Blitz")]
        public AudioClip blitzTimerEndSound;
        public AudioClip blitzWonSound;
        public AudioClip doorSlamSound;

        [Header("Achievement")] 
        public AudioClip achievementSound;

        [Header("Easter Egg")] 
        public AudioClip napoliSound;
    }
}