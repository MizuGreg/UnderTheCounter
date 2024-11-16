using UnityEngine;

namespace Technical
{
    public abstract class QuitGame
    {
        public static void Quit()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    }
}
