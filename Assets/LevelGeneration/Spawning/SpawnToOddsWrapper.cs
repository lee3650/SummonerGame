using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpawnToOddsWrapper
{
    public List<SpawnToProbability> Spawns;

    public List<SpawnToProbability> GetEnabledSpawns()
    {
        List<SpawnToProbability> result = new List<SpawnToProbability>();

        foreach (SpawnToProbability s in Spawns)
        {
            if (s.RequiredChange == GameplayChange.None || LetterManager.UseGameplayChange(s.RequiredChange))
            {
                result.Add(s);
            }
        }

        return result; 
    }
}
