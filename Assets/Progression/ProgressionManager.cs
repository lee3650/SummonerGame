﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    [SerializeField] LevelUnlocks[] ProgressionRewards; //so, index = level unlocked
    private static LevelUnlocks[] StaticProgRewards = new LevelUnlocks[0];

    private void Awake()
    {
        StaticProgRewards = ProgressionRewards;
    }

    public List<DisplayRewardData> GetRewardDataAtLevel(int level) 
    {
        return ProgressionRewards[level].GetRewardData();
    }

    public static List<GameObject> GetItemsUnlockedAtLevel(int level)
    {
        if (StaticProgRewards.Length == 0)
        {
            return new List<GameObject>();
        }

        List<GameObject> result = new List<GameObject>();

        for (int i = 0; i <= level; i++)
        {
            result.AddRange(StaticProgRewards[i].GetUnlockedItems());
        }

        return result; 
    }
}