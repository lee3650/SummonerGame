using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    private Item MyItem;
    [SerializeField] protected Image Image;
    [SerializeField] Sprite DefaultSprite;
    [SerializeField] Color DefaultColor = new Color(1, 1, 1, 0);
    public int Index;

    public event System.Action<ItemSlot> OnSlotClicked = delegate { };

    public void SetItem(Item item)
    {
        MyItem = item;
        Image.sprite = item.GetInventorySprite();
        Image.color = item.GetColor();
    }
    
    public void OnPointerClick(PointerEventData data)
    {
        OnSlotClicked(this);
    }

    public Item GetItem()
    {
        return MyItem;
    }
    
    public void ResetItem()
    {
        Image.sprite = DefaultSprite;
        MyItem = null;
        Image.color = DefaultColor;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
