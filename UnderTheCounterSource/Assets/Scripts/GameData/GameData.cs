using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ShopWindow;
using UnityEngine;

[SuppressMessage("ReSharper", "CheckNamespace")]
public static class GameData
{
    // Initialization
    public static GameLog Log = new();
    public static GameChoices Choices = new();
        
    public static string BarName = "The Chitchat";
    public static float DailyTime = 180;
    public static int CurrentDay = 1;
    public static int DrunkCustomers = 0;
    public static int MaxDrunkCustomers = 4;
    public static List<Poster> CurrentPosters = new();
    public static float Savings = 100;
    public static float TodayEarnings = 0;
    public static int Rent = 20;
    public static int Food = Random.Range(5, 10);
    public static int Supplies = Random.Range(10, 15);

    public static void StartDay()
    {
        DrunkCustomers = 0;
        switch (CurrentDay)
        {
            case 1:
                DailyTime = 0;
                MaxDrunkCustomers = 99;
                Savings = 100;
                Rent = 20;
                Food = Random.Range(5, 10);
                Supplies = Random.Range(10, 15);
                break;
            case 2:
                DailyTime = 240;
                MaxDrunkCustomers = 99;
                Rent = 20;
                Food = Random.Range(5, 10);
                Supplies = Random.Range(10, 15);
                break;
            default:
                DailyTime = 270;
                MaxDrunkCustomers = 4;
                Rent = 20;
                Food = Random.Range(5, 10);
                Supplies = Random.Range(15, 30);
                break;
        }
    }

    public static bool IsPosterActive(int posterID)
    {
        foreach (Poster poster in CurrentPosters)
        {
            if (poster.posterID == posterID) return true;
        }
        return false;
    }

    public static void EndDay(float dailyBalance)
    {
        Savings += dailyBalance;
        TodayEarnings = 0;
        CurrentDay++;
        CurrentPosters.Clear(); // Removes all posters/effects
    }
}