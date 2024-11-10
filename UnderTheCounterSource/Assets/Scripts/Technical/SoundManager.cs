using UnityEngine;

namespace Technical
{
    public class SoundManager : MonoBehaviour
    {
        // todo. will need to be a singleton. will use events to play sounds
        
        private static SoundManager instance;

        public void Awake()
        {
            instance = this;
        }

        public static void playSound()
        {
            // todo. will retrieve from a dictionary in the future, easier to work with than a thousand functions
        }

        public SoundManager()
        {
            EventSystemManager.OnCustomerEnter += onCustomerEnterSound;
            EventSystemManager.OnDayEnd += onDayEndSound;
        }

        public void onCustomerEnterSound()
        {
            // todo
        }

        public void onDayEndSound()
        {
            // todo
        }
    }
}
