using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SavedGameData;
using UnityEngine;

namespace Mailbox
{
    public class MailboxManager : MonoBehaviour
    {
        [Header("Canvas Groups")]
        //[SerializeField] private CanvasGroup mainCanvas;
        //[SerializeField] private CanvasGroup continueButtonCanvas;
        //[SerializeField] private CanvasGroup containerCanvas;
        [SerializeField] private CanvasGroup letterCanvas;
        [SerializeField] private CanvasGroup BUCanvas;
        [SerializeField] private CanvasGroup theaterCanvas;
        [SerializeField] private CanvasGroup newspaperCanvas;
        [SerializeField] private CanvasGroup voteCanvas;
        
        private List<Mailbox> _mailboxes;
        private Mailbox _currentMailbox;
        
        private void Start()
        {
            Initialize();
            
            LoadMailbox();
            
            SetCurrentElements();
        }

        private void Initialize()
        {
            DeactivateElement(letterCanvas);
            DeactivateElement(BUCanvas);
            DeactivateElement(theaterCanvas);
            DeactivateElement(newspaperCanvas);
            DeactivateElement(voteCanvas);
        }

        private void LoadMailbox()
        {
            // Read Mailboxes JSON and create mailboxes list
            string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/MailboxData/Mailboxes.json");
            
            _mailboxes = JsonConvert.DeserializeObject<MailboxList>(jsonString).mailboxes;
            
            // Get the mailbox based on current day
            //_currentMailbox = _mailboxes.Find(a => a.day == GameData.CurrentDay);
            // DEBUG
            _currentMailbox = _mailboxes.Find(a => a.day == 2);
        }

        private void SetCurrentElements()
        {
            if(_currentMailbox.letter) ActivateElement(letterCanvas);
            if(_currentMailbox.BU) ActivateElement(BUCanvas);
            if(_currentMailbox.theater) ActivateElement(theaterCanvas);
            if(_currentMailbox.newspaper) ActivateElement(newspaperCanvas);
            if(_currentMailbox.vote) ActivateElement(voteCanvas);
        }

        private void ActivateElement(CanvasGroup cg)
        {
            cg.alpha = 1f;
            cg.blocksRaycasts = true;
            cg.interactable = true;
        }

        private void DeactivateElement(CanvasGroup cg)
        {
            cg.alpha = 0f;
            cg.blocksRaycasts = false;
            cg.interactable = false;
        }
        
    }
}
