﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this will have to be in the main menu as well so it's guaranteed to run. 
public class ExperienceManager : MonoBehaviour
{
    private static int CurrentLevel = 0;
    private static float CurrentLevelXP = 0f;
    
    private const string KeyToCurrentLevel = "CurLevl";
    private const string KeyToCurrentLevelXP = "CurLevlXP";

    private static bool exitingLevel = false;

    public const float FirstTwoLevelXP = 4f;

    private static List<XPMessage> xpToApply = new List<XPMessage>();

    private void Awake()
    {
        CurrentLevel = PlayerPrefs.GetInt(KeyToCurrentLevel, 0);
        CurrentLevelXP = PlayerPrefs.GetFloat(KeyToCurrentLevelXP, 0);
    }
    
    public static float GetCurrentLevelXP()
    {
        return CurrentLevelXP;
    }

    public static bool GainXP(float amt)
    {
        bool changedLevel = false;

        CurrentLevelXP += amt;
        float reqXP = GetXPToNextLevel(CurrentLevel);
        if (CurrentLevelXP >= reqXP)
        {
            changedLevel = true;
            CurrentLevel++;
            CurrentLevelXP -= reqXP;
        }

        return changedLevel;
    }

    public static int GetCurrentLevel()
    {
        return CurrentLevel;
    }

    public static float GetCurrentLevelPercentage()
    {
        return CurrentLevelXP / GetXPToNextLevel(CurrentLevel);
    }

    public static void ResetXPMessages()
    {
        xpToApply = new List<XPMessage>();
    }

    public static List<XPMessage> GetXPMessages()
    {
        return xpToApply;
    }
    public static void AddXPMessage(XPMessage message)
    {
        xpToApply.Add(message);
    }

    public static float GetXPToNextLevel(int currentLevel)
    {
        print("XP is not implemented!");
        return 10f;
    }

    public static void SetExitingLevel(bool value)
    {
        exitingLevel = value;
    }

    public static bool ExitingLevel()
    {
        return exitingLevel;
    }
}