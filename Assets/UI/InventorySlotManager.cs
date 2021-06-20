using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotManager : MonoBehaviour
{
    [SerializeField] Inventory Inventory; //so, for now we're serializing the inventory it refers to. All these references may cause issues later. 

    [SerializeField] Transform Content;

    [SerializeField] InventoryRow InventoryRowPrefab;
    [SerializeField] InventorySlot InventorySlotPrefab;

    [SerializeField] int ItemsPerRow = 6; //can I make it const? 
    [SerializeField] int TotalRows = 0; //start with 0 rows. 

    private List<InventorySlot> InventorySlots = new List<InventorySlot>();
    private List<InventoryRow> InventoryRows = new List<InventoryRow>();
    
    private void Start()
    {
        //I guess on start? Sure, I guess. 
        Inventory.ItemDropped += Inventory_ItemDropped;
        Inventory.ItemPickedUp += Inventory_ItemPickedUp;
    }

    private void Inventory_ItemPickedUp(Item obj)
    {
        InventorySlot newSlot = Instantiate<InventorySlot>(InventorySlotPrefab);
        newSlot.SetItem(obj);
        InventorySlots.Add(newSlot);

        DisplayInventorySlots();
    }

    private void Inventory_ItemDropped(Item obj)
    {
        //so, we need to find the slot associated with that item, and destroy it,
        //then redisplay. 

        for (int i = InventorySlots.Count - 1; i >= 0; i--)
        {
            if (InventorySlots[i].GetItem() == obj)
            {
                InventorySlot s = InventorySlots[i];
                InventorySlots.RemoveAt(i);
                Destroy(s.gameObject);
                DisplayInventorySlots();
                break;
            }
        }
    }

    private void DisplayInventorySlots()
    {
        //so, first destroy our current display. 
        DestroyCurrentDisplay();

        int rowNum = Mathf.CeilToInt((float)InventorySlots.Count / (float)ItemsPerRow);

        print("row num: " + rowNum);

        for (int i = 0; i < rowNum; i++)
        {
            InventoryRow row = Instantiate<InventoryRow>(InventoryRowPrefab, Content);

            for (int x = 0; x < ItemsPerRow; x++)
            {
                int index = (i * ItemsPerRow) + x;

                if (index < InventorySlots.Count)
                {
                    row.AddSlot(InventorySlots[index]);
                }

            }

            InventoryRows.Add(row);
        }
    }

    private void DestroyCurrentDisplay()
    {
        foreach (InventoryRow row in InventoryRows)
        {
            row.RemoveAllSlots();
            Destroy(row.gameObject);
        }
        
        InventoryRows = new List<InventoryRow>();
    }

    public void ShowSlotMenu(InventorySlot slot)
    {

    }
}
