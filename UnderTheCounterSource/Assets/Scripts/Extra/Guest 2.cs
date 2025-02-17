using System;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace Extra {
    [Serializable]
    public class GuestList
    {
        public List<Guest> guests;
    }

    [Serializable]
    public class Guest {
        public bool isUnlocked;
        public int index;
        public string nickname;
        public string name;
        public string age;
        public string height;
        public string status;
        public string job;
        public string favouriteDrink;
        public string description;

        public override string ToString() {
            try {
                return $"isUnlocked: {isUnlocked}, index: {index}, nickname: {nickname}, Name: {name}, Age: {age}, Height: {height}, Status: {status}, Job: {job}, Favourite Drink: {favouriteDrink}, Description: {description}";
            } catch (Exception e) {
                return $"Error :( Exception in Guest.ToString(): {e}";
            }
        }
    }
}