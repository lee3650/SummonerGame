﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedItemManager : MonoBehaviour
{
    [SerializeField] PlayerInventory PlayerInventory;

    private void Start()
    {
        int cur = ExperienceManager.GetCurrentLevel();
        List<GameObject> items = ProgressionManager.GetItemsUnlockedAtLevel(cur);
        foreach (GameObject item in items)
        {
            GameObject instance = Instantiate(item);
            PlayerInventory.TryToPickUpGameobject(instance);
        }
    }
}
