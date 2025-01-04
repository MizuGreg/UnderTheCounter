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
        public bool visible;

        public Poster(int posterID, Sprite image, string name, float price, string buff, string nerf,
            string description, int hanged, bool visible)
        {
            this.posterID = posterID;
            this.image = image;
            this.name = name;
            this.price = price;
            this.buff = buff;
            this.nerf = nerf;
            this.description = description;
            this.hanged = hanged;
            this.visible = visible;
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

        public PosterData(int id, float price, int hanged, bool visible)
        {
            this.id = id;
            this.price = price;
            this.hanged = hanged;
            this.visible = visible;
        }
    }
}