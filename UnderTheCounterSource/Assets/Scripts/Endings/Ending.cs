using System;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace Endings {
    [Serializable]
    public class EndingList
    {
        public List<Ending> endings;
    }

    [Serializable]
    public class Ending {
        public string type;
        public List<string> lines;

        public override string ToString() {
            try {
                return $"type: {type}, lines: {lines[0]}";
            } catch (Exception e) {
                return $"Error :( Exception in Ending.ToString(): {e}";
            }
        }
    }
}