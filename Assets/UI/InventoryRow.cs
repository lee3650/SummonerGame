using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryRow : MonoBehaviour
{
    InventorySlot[] InventorySlots;
    int CurrentSlot = 0; 

    public void Awake() //awake should work. 
    {
        InventorySlots = GetComponentsInChildren<InventorySlot>();
    }
        
    public void FillFirstAvailableSlot(Item item)
    {
        GetFirstAvailableSlot().SetItem(item);
        CurrentSlot++;
    }

    public InventorySlot GetFirstAvailableSlot()
    {
        return InventorySlots[CurrentSlot];
    }
}
