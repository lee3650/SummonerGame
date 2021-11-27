using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LetterManager : MonoBehaviour
{
    [SerializeField] XPApplier XPApplier;
    [SerializeField] LetterPanel LetterPanel;
    [SerializeField] LetterUnlock[] LetterUnlocks;

    private static int TotalIslands = -1;

    private const string islandPath = "islands";

    private static Dictionary<GameplayChange, bool> GameplayChangeMap = null;
    private static List<SpawnToProbability> ExtraSpawns = null;

    private void Awake()
    {
        XPApplier.FinishedXPApply += ApplyLetters;
        if (TotalIslands == -1)
        {
            LoadTotalIslands();
        }

        if (GameplayChangeMap == null)
        {
            InitializeGameplayChanges();
        }

        if (ExtraSpawns == null)
        {
            InitializeExtraSpawns();
        }
    }

    private void InitializeExtraSpawns()
    {
        ExtraSpawns = new List<SpawnToProbability>();

        foreach (LetterUnlock l in LetterUnlocks)
        {
            if (l.Type == LetterType.AddSpawn && l.TotalIslandsReq <= TotalIslands)
            {
                ExtraSpawns.AddRange(l.EnemiesAdded);
            }
        }
    }

    private void InitializeGameplayChanges()
    {
        GameplayChangeMap = new Dictionary<GameplayChange, bool>();

        foreach (LetterUnlock l in LetterUnlocks)
        {
            if (l.Type == LetterType.GameplayChange && l.TotalIslandsReq <= TotalIslands)
            {
                GameplayChangeMap[l.GameplayChange] = true;
            }
        }
    }

    public static List<SpawnToProbability> GetExtraSpawns()
    {
        return ExtraSpawns;
    }

    private void LoadTotalIslands()
    {
        string path = Application.persistentDataPath + islandPath;
        if (File.Exists(path))
        {
            string islands = File.ReadAllText(path);
            TotalIslands = int.Parse(islands.Trim());
        } else
        {
            TotalIslands = 0;
        }
    }

    private static void SaveTotalIslands()
    {
        File.WriteAllText(Application.persistentDataPath + islandPath, "" + TotalIslands);
    }

    private void ApplyLetters()
    {
        TotalIslands += 1;

        print("Total islands: " + TotalIslands);

        foreach (LetterUnlock l in LetterUnlocks)
        {
            if (l.TotalIslandsReq == TotalIslands) //since we just added, the only way that's possible is it just got unlocked
            {
                UnlockedLetter(l);
            }
        } 

        SaveTotalIslands();
    }

    private void UnlockedLetter(LetterUnlock l)
    {
        if (l.Type == LetterType.GameplayChange)
        {
            GameplayChangeMap[l.GameplayChange] = true;
        }
        else if (l.Type == LetterType.AddSpawn)
        {
            ExtraSpawns.AddRange(l.EnemiesAdded);
        }

        LetterPanel.ShowLetter(l);
    }

    public static bool UseGameplayChange(GameplayChange change)
    {
        if (GameplayChangeMap == null)
        {
            return false;
        }

        bool value = false;

        if (GameplayChangeMap.TryGetValue(change, out value))
        {
            return value;
        }

        return value;
    }

    public static void ResetState()
    {
        GameplayChangeMap = null;
        ExtraSpawns = null;
        TotalIslands = 0;
        SaveTotalIslands();
    }
}
