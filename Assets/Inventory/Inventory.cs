using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> Items = new List<Item>();

    public event Action<Item> ItemPickedUp = delegate { };
    public event Action<Item> ItemDropped = delegate { };

    public void PickUpItem(Item item)
    {
        item.OnPickup(transform);
        Items.Add(item); //pretty much it. 
        ItemPickedUp(item);
    }

    public int GetTotalItems()
    {
        return Items.Count;
    }

    //so, actually, this would be called from the UI, which is a little weird. 
    public void DropItem(Item item)
    {
        Items.Remove(item);
        item.OnDrop();
        ItemDropped(item);
    }
}
