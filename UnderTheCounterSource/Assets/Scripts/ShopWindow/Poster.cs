using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace ShopWindow
{
    
    // Serializable class to store important poster data
    [System.Serializable]
    public class Poster
    {
        public int id;
        public float price;
        public int hanged;
        public bool visible;
        public string name; // Reference to the text component for the poster name
        public string buff; // Reference to the poster buff percentage
        public string nerf; // Reference to the poster nerf percentage
        public string description; // Reference to the text for the poster description
        public bool locked; // Indicates if the poster is locked

        public Poster(int id, float price, int hanged, bool locked, bool visible, string name, string buff, string nerf, string description)
        {
            this.id = id;
            this.price = price;
            this.hanged = hanged;
            this.visible = visible;
            this.name = name;
            this.buff = buff;
            this.nerf = nerf;
            this.description = description;
            this.locked = locked;
        }
    }
}