using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private Item MyItem;
    [SerializeField] Button Button;
    [SerializeField] Image Image;
    
    public void SetItem(Item item)
    {
        MyItem = item;
    }

    public void EnableAll()
    {
        Button.enabled = true;
        Image.enabled = true;
    }

    public Item GetItem()
    {
        return MyItem;
    }

}
