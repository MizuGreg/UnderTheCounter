using System.Collections;
using System.Collections.Generic;
using Technical;
using UnityEngine;

public class ShowDebug : MonoBehaviour
{
    public void Toggle()
    {
        GetComponent<DebugLogger>().enabled = !GetComponent<DebugLogger>().enabled;
    }
}
