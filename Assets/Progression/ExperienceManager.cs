using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this will have to be in the main menu as well so it's guaranteed to run. 
public class ExperienceManager : MonoBehaviour
{
    private static int CurrentLevel = 0;
    private static float CurrentLevelXP = 0f;
    
    private const string KeyToCurrentLevel = "CurLevl";
    private const string KeyToCurrentLevelXP = "CurLevlXP";

    public const float FirstTwoLevelXP = 4f;

    private void Awake()
    {
        CurrentLevel = PlayerPrefs.GetInt(KeyToCurrentLevel, 0);
        CurrentLevelXP = PlayerPrefs.GetFloat(KeyToCurrentLevelXP, 0);
    }
    
    public static void GainXP(float amt)
    {
        CurrentLevelXP += amt;
        float reqXP = GetXPToNextLevel(CurrentLevel);
        if (CurrentLevelXP >= reqXP)
        {
            CurrentLevel++;
            CurrentLevelXP -= reqXP;
        }
    }

    public static int GetCurrentLevel()
    {
        return CurrentLevel;
    }

    public static float GetCurrentLevelPercentage()
    {
        return CurrentLevelXP / GetXPToNextLevel(CurrentLevel);
    }

    private static float GetXPToNextLevel(int currentLevel)
    {
        print("XP is not implemented!");
        return 10f;
    }
}