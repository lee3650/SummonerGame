using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryRow : MonoBehaviour
{
    public void AddSlot(InventorySlot slot)
    {
        slot.transform.parent = transform;
        slot.transform.localScale = new Vector3(1, 1, 1);
        slot.EnableAll();
    }
        
    public void RemoveAllSlots()
    {
        foreach (Transform t in transform)
        {
            t.parent = null;
        }
    }
}
