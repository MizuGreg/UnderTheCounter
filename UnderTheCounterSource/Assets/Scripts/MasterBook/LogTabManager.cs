using System;
using System.Collections.Generic;
using SavedGameData;
using Technical;
using TMPro;
using UnityEngine;

namespace MasterBook
{
    public class LogTabManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI dayText;
        [SerializeField] private List<GameObject> logEntries = new();

        public void SetDay()
        {
            dayText.text = $"DAY {GameData.CurrentDay}";
        }
        
        public void PopulateLogPages()
        {
            List<Tuple<string,string>> entriesToWrite = new();
            for (int i = logEntries.Count; i >= 0; i--) // goes from 8 (at the time this was written) to 0
            {
                try
                {
                    print($"i: {i}");
                    entriesToWrite.Add(GameData.Log[^(i)]); // retrieve log entries as a pile, starting from the last
                }
                catch (ArgumentOutOfRangeException)
                {
                    continue; // breaks for loop; if this happens it means we have fewer than 9 log entries
                }
            }

            for (int i = 0; i < logEntries.Count; i++)
            {
                GameObject entryObject = logEntries[i];
                if (i < entriesToWrite.Count)
                {
                    entryObject.transform.Find("LogName").GetComponent<TextMeshProUGUI>().text = entriesToWrite[i].Item1;
                    entryObject.transform.Find("LogText").GetComponent<TextMeshProUGUI>().text = entriesToWrite[i].Item2;
                    entryObject.SetActive(true);
                }
                else
                {
                    entryObject.SetActive(false);
                }
            }
        }

        public void TurnPage()
        {
            EventSystemManager.OnPageTurned();
        }
    }
}