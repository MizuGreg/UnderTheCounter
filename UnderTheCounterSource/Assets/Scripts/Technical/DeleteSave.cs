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
        GameData.DeleteSave();
        GetComponent<AchievementManager>().ResetAchievements();
        GameObject.Find("GuestBookManager").GetComponent<GuestBookManager>().ResetGuests();
    }
}
