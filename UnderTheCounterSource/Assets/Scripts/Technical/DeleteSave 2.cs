using System.Collections;
using System.Collections.Generic;
using Achievements;
using SavedGameData;
using UnityEngine;
using Extra;

public class DeleteSave : MonoBehaviour
{
    public void DeleteAll()
    {
        // some debug information
        Debug.Log("Deleting save file...");
        GameData.DeleteSave();

        GetComponent<AchievementManager>().ResetAchievements();
        Debug.Log("Achievements deleted!");

        GameObject.Find("GuestBookManager").GetComponent<GuestBookManager>().ResetGuests();
        Debug.Log("Guests deleted!");
    }
}
