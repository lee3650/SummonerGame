using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotManager : MonoBehaviour
{
    [SerializeField] InventoryRow InventoryRow;
    [SerializeField] Inventory Inventory; //so, for now we're serializing the inventory it refers to. All these references may cause issues later. 

    [SerializeField] int ItemsPerRow = 6; //can I make it const? 
    [SerializeField] int TotalRows = 1; //start with 1 row. 

    public void Start()
    {
        //I guess on start? Sure, I guess. 
        Inventory.ItemDropped += Inventory_ItemDropped;
        Inventory.ItemPickedUp += Inventory_ItemPickedUp;
    }

    private void Inventory_ItemPickedUp(Item obj)
    {
        //so, we count our total items, right. 
        
        if (Inventory.GetTotalItems() > TotalRows * ItemsPerRow)
        {
            //so, get a new row. 
            //we're going to need an inventory row script. 
            //this is kind of a weird way to do it. Why not just add one at a time? 
            //eh, whatever. 
        }
    }

    private void Inventory_ItemDropped(Item obj)
    {
        //should we just not delete rows? No, I think it's fun to delete them. 
    }

    public void ShowSlotMenu(InventorySlot slot)
    {

    }
}
