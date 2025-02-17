using System;
using System.Collections.Generic;

namespace Mailbox
{
    [Serializable]
    public class Mailbox
    {
        public int day;
        public bool letter;
        public bool BU;
        public bool theater;
        public bool newspaper;
        public bool vote;
        
        public override string ToString()
        {
            try
            {
                return $"day: {day}, letter: {letter}, BU: {BU}, theater: {theater}, newspaper: {newspaper}, vote: {vote}";
            }
            catch (Exception e)
            {
                return $"Error :( Exception in Mailbox.ToString(): {e}";
            }
        }
    }

    [Serializable]
    public class MailboxList
    {
        public List<Mailbox> mailboxes;
    }
}