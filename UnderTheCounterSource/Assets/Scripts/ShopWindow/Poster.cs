using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace ShopWindow
{
    
    [Serializable]
    public class Poster
    {
        // private static int maxPosterID = 0;

        public int posterID;
        public Sprite image;
        public string name;
        public float price;
        public string buff;
        public string nerf;
        public string description;
        public int hanged;

        public Poster(int posterID, Sprite image, string name, float price, string buff, string nerf,
            string description, int hanged)
        {
            this.posterID = posterID;
            this.image = image;
            this.name = name;
            this.price = price;
            this.buff = buff;
            this.nerf = nerf;
            this.description = description;
            this.hanged = hanged;
        }
    }
    
    // Serializable class to store important poster data
    [System.Serializable]
    public class PosterData
    {
        public int id;
        public float price;
        public int hanged;
        public bool visible;
        public Sprite image; // Reference to the image component for the poster
        public string name; // Reference to the text component for the poster name
        public string buff; // Reference to the poster buff percentage
        public string nerf; // Reference to the poster nerf percentage
        public string description; // Reference to the text for the poster description
        public bool isLocked; // Indicates if the poster is locked

        public PosterData(int id, float price, int hanged, bool visible, Sprite image, string name, string buff, string nerf, string description, bool isLocked)
        {
            this.id = id;
            this.price = price;
            this.hanged = hanged;
            this.visible = visible;
            this.image = image;
            this.name = name;
            this.buff = buff;
            this.nerf = nerf;
            this.description = description;
            this.isLocked = isLocked;
        }
    }
}