using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelUnlocks
{
    public ProgressionUnlock[] Unlocks;

    public List<DisplayRewardData> GetRewardData()
    {
        List<DisplayRewardData> result = new List<DisplayRewardData>();

        foreach (ProgressionUnlock u in Unlocks)
        {
            result.Add(u.DisplayRewardData);
        }

        return result;
    }

    public List<GameObject> GetUnlockedItems()
    {
        List<GameObject> result = new List<GameObject>();

        foreach (ProgressionUnlock u in Unlocks)
        {
            if (u.IsItem)
            {
                result.Add(u.Item);
            }
        }

        return result; 
    }
}
