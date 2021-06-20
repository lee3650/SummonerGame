using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    private Item MyItem;

    public void SetItem(Item item)
    {
        MyItem = item;
    }

}
