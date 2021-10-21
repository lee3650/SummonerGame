using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this will have to be in the main menu as well so it's guaranteed to run. 
public class ExperienceManager : MonoBehaviour
{
    private static int CurrentLevel;
    private static float CurrentLevelPercentage;
    
    private const string KeyToCurrentLevel = "CurLevl";
    private const string KeyToCurrentLevelPercentage = "CurLevlPct";


    private void Awake()
    {
        CurrentLevel = PlayerPrefs.GetInt(KeyToCurrentLevel, 0);
        CurrentLevelPercentage = PlayerPrefs.GetFloat(KeyToCurrentLevelPercentage, 0);
    }

    public static int GetCurrentLevel()
    {
        return CurrentLevel;
    }
    public static float GetCurrentLevelPercentage()
    {
        return CurrentLevelPercentage;
    }

    private static float GetXPToNextLevel(int currentLevel)
    {
        print("XP is not implemented!");
        return 10f;
    }
}
