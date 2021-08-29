using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemReward : Reward
{
    [SerializeField] PlayerInventory PlayerInventory;
    [SerializeField] Item Item;

    public override void ApplyReward()
    {
        Item item = Instantiate(Item);
        PlayerInventory.PickUpItem(item);
    }
}
