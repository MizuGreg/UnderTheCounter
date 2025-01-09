using System.Collections;
using System.Collections.Generic;
using Achievements;
using SavedGameData;
using UnityEngine;

public class DeleteSave : MonoBehaviour
{
    public void DeleteAll()
    {
        GameData.DeleteSave();
        GetComponent<AchievementManager>().ResetAchievements();
    }
}
