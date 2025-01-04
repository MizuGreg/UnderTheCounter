using System.Collections;
using UnityEngine;

namespace Technical
{
    public class DebugLogger : MonoBehaviour
    {
        private const uint QueueSize = 10; // number of messages to keep
        private readonly Queue logQueue = new();

        private void Start() {
            Debug.Log("Started up logging.");
        }

        private void OnEnable() {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable() {
            Application.logMessageReceived -= HandleLog;
        }

        private void HandleLog(string logString, string stackTrace, LogType type) {
            logQueue.Enqueue("[" + type + "] : " + logString);
            if (type == LogType.Exception)
                logQueue.Enqueue(stackTrace);
            while (logQueue.Count > QueueSize)
                logQueue.Dequeue();
        }

        private void OnGUI() {
            GUILayout.BeginArea(new Rect(Screen.width - 0.2f*Screen.width, 0, 0.2f*Screen.width - 10, Screen.height));
            GUILayout.Label("\n" + string.Join("\n", logQueue.ToArray()));
            GUILayout.EndArea();
        }
    }
}