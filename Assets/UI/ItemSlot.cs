using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    private Item MyItem;
    [SerializeField] protected Image Image;

    public void SetItem(Item item)
    {
        MyItem = item;
        Image.sprite = item.GetInventorySprite();
        Image.color = item.GetColor();
    }

    public Item GetItem()
    {
        return MyItem;
    }

    public void ResetItem()
    {
        Image.sprite = null;
        MyItem = null;
        Image.color = Color.white;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
