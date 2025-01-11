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
        // [SerializeField] private Color normalCustomerColor = new(0.1f, 0.4f, 0.5f);
        [SerializeField] private Color margaretColor = new(0.1f, 0.4f, 0.5f);
        [SerializeField] private Color kathrineColor = new(0.1f, 0.4f, 0.5f);
        [SerializeField] private Color heleneColor = new(0.1f, 0.4f, 0.5f);
        [SerializeField] private Color ernestColor = new(0.1f, 0.4f, 0.5f);
        [SerializeField] private Color charlesColor = new(0.1f, 0.4f, 0.5f);
        [SerializeField] private Color bettyColor = new(0.1f, 0.4f, 0.5f);
        [SerializeField] private Color eugeneColor = new(0.1f, 0.4f, 0.5f);
        [SerializeField] private Color dorisColor = new(0.1f, 0.4f, 0.5f);
        [SerializeField] private Color willieColor = new(0.1f, 0.4f, 0.5f);
        [SerializeField] private Color gastonColor = new(0.1f, 0.4f, 0.5f);
        [SerializeField] private Color kennethColor = new(0.1f, 0.4f, 0.5f);
        [SerializeField] private Color lukeColor = new(0.1f, 0.4f, 0.5f);
        [SerializeField] private Color bartenderColor = new(0f, 0f, 0f);
        [SerializeField] private Color inspectorColor = new(0.6f, 0.2f, 0f);
        [SerializeField] private Color unionColor = new(0.3f, 0.5f, 0f);
        [SerializeField] private Color mafiaColor = new(0.3f, 0.1f, 0.5f);
        
        [SerializeField] private TextMeshProUGUI dayText;
        [SerializeField] private List<GameObject> logEntries = new();

        public void SetDay()
        {
            dayText.text = $"DAY {GameData.CurrentDay}";
        }
        
        public void PopulateLogPages()
        {
            List<Tuple<string,string>> entriesToWrite = new();
            for (int i = logEntries.Count; i > 0; i--) // goes from 8 (at the time this was written) to 1
            {
                try
                {
                    entriesToWrite.Add(GameData.Log[^i]); // retrieve log entries as a pile, starting from the last
                }
                catch (ArgumentOutOfRangeException)
                {
                    continue; // breaks for loop; if this happens it means we have fewer than 8 log entries
                }
            }

            for (int i = 0; i < logEntries.Count; i++)
            {
                GameObject entryObject = logEntries[i];
                if (i < entriesToWrite.Count)
                {
                    TextMeshProUGUI logName = entryObject.transform.Find("LogName").GetComponent<TextMeshProUGUI>();
                    logName.text = entriesToWrite[i].Item1;
                    ColorName(logName, entriesToWrite[i].Item1);
                    
                    TextMeshProUGUI logText = entryObject.transform.Find("LogText").GetComponent<TextMeshProUGUI>();
                    logText.text = entriesToWrite[i].Item2;
                    
                    entryObject.SetActive(true);
                }
                else
                {
                    entryObject.SetActive(false);
                }
            }
        }

        private void ColorName(TextMeshProUGUI name, string text)
        {
            if (text is "Bartender") name.color = bartenderColor;
            else if (text.Contains("Mafia")) name.color = mafiaColor;
            else if (text.Contains("Inspector") || text.Contains("Howard")) name.color = inspectorColor;
            else if (text.Contains("Union") || text.Contains("BU")) name.color = unionColor;
            else if (text.Contains("Margaret")) name.color = margaretColor;
            else if (text.Contains("Kathrine")) name.color = kathrineColor;
            else if (text.Contains("Helene")) name.color = heleneColor;
            else if (text.Contains("Ernest")) name.color = ernestColor;
            else if (text.Contains("Charles")) name.color = charlesColor;
            else if (text.Contains("Betty")) name.color = bettyColor;
            else if (text.Contains("Eugene")) name.color = eugeneColor;
            else if (text.Contains("Doris")) name.color = dorisColor;
            else if (text.Contains("Willie")) name.color = willieColor;
            else if (text.Contains("Gaston")) name.color = gastonColor;
            else if (text.Contains("Kenneth")) name.color = kennethColor;
            else if (text.Contains("Luke")) name.color = lukeColor;
        }

        public void TurnPage()
        {
            EventSystemManager.OnPageTurned();
        }
    }
}