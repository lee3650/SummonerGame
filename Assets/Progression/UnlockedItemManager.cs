using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedItemManager : MonoBehaviour
{
    [SerializeField] PlayerInventory PlayerInventory;
    
    private void Awake()
    {
        int cur = ExperienceManager.GetCurrentLevel();
        for (int i = cur; i >= 0; i--)
        {
            List<GameObject> items = ItemProgressionManager.GetItemsUnlockedAtLevel(cur);
            foreach (GameObject item in items)
            {
                PlayerInventory.TryToPickUpGameobject(item);
            }
        }
    }
}
