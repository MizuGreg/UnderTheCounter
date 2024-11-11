using UnityEngine;

namespace Technical
{
    public class QuitGame
    {
        // absolutely temporary class! bad name, bad location, bad namespace. needs to be refactored in the future
        public static void Quit()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    }
}
