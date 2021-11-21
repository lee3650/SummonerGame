using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//this will have to be in the main menu as well so it's guaranteed to run. 
public class ExperienceManager : MonoBehaviour
{
    private static int CurrentLevel = 0;
    private static float CurrentLevelXP = 0;
    
    private const string KeyToCurrentLevel = "CurLevl";
    private const string KeyToCurrentLevelXP = "CurLevlXP";

    private static bool exitingLevel = true;

    public static event Action LeveledUp = delegate { };

    private static List<XPMessage> xpToApply = new List<XPMessage>();

    private void Awake()
    {
       CurrentLevel = PlayerPrefs.GetInt(KeyToCurrentLevel, 0);
       CurrentLevelXP = PlayerPrefs.GetFloat(KeyToCurrentLevelXP, 0);
    }
    
    public static void ResetState()
    {
        ResetXPMessages();

        CurrentLevel = 0;
        CurrentLevelXP = 0f;

        PlayerPrefs.SetInt(KeyToCurrentLevel, 0);
        PlayerPrefs.SetFloat(KeyToCurrentLevelXP, 0f);
    }

    public static void WriteXP()
    {
        PlayerPrefs.SetInt(KeyToCurrentLevel, GetCurrentLevel());
        PlayerPrefs.SetFloat(KeyToCurrentLevelXP, GetCurrentLevelXP());
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
            LeveledUp();
        }

        return changedLevel;
    }

    public static IEnumerator AnimateXPToZero()
    {
        float cur = CurrentLevelXP;

        float delta = 0.05f;

        float accum = 0f;

        for (int i = 0; i < 1 / delta; i++)
        {
            CurrentLevelXP = Mathf.Lerp(cur, 0f, accum);
            accum += delta;
            yield return new WaitForSeconds(delta);
        }

        CurrentLevelXP = 0f;
        WriteXP();
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
        //So, f(0) = like, 5 ish.
        //f(1) = 6. 
        //Let's do current level squared? 

        float[] XPToNextLevel = new float[]
        {
            5, 
            6,
            15, //2 islands: unlock sand
            20, //2 islands: unlock arrow turner
            40, //3 islands: random prices
            40, //2 islands: archers
            30, //3 islands: unlock increments
            50,
            50,
            50, 
        };

        if (currentLevel < XPToNextLevel.Length) 
        { 
            return XPToNextLevel[currentLevel];
        }
        return 50 + currentLevel;
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