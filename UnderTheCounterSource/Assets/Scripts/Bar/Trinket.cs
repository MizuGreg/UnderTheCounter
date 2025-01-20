using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bar
{
    [Serializable]
    public class Trinket
    {
        public int id;
        public string title;
        public string caption;
        
        public override string ToString()
        {
            try
            {
                return $"id: {id}, title: {title}, caption: {caption}";
            }
            catch (Exception e)
            {
                return $"Error :( Exception in Trinket.ToString(): {e}";
            }
        }
    }

    [Serializable]
    public class TrinketList
    {
        public List<Trinket> trinkets;
    }
}