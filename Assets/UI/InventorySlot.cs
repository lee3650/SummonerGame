using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : ItemSlot
{
    [SerializeField] Button Button;
    private InventorySlotManager InventorySlotManager;

    public void SetManager(InventorySlotManager inventorySlotManager)
    {
        InventorySlotManager = inventorySlotManager;
    }

    public void EnableAll()
    {
        Button.enabled = true;
        Image.enabled = true;
    }

    public void OnClick()
    {
        InventorySlotManager.SlotSelected(this);
    }
}
