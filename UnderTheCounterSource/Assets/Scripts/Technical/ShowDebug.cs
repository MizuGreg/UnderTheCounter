using UnityEngine;

namespace Technical
{
    public class ShowDebug : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<DebugLogger>().enabled = PlayerPrefs.GetInt("ShowDebug") == 1;
        }

        public void Toggle()
        {
            GetComponent<DebugLogger>().enabled = !GetComponent<DebugLogger>().enabled;
            PlayerPrefs.SetInt("ShowDebug", GetComponent<DebugLogger>().enabled ? 1 : 0);
        }
    }
}
