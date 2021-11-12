using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerUpHandler
{
    private Item MyItem = null;
    [SerializeField] protected Image Image;
    [SerializeField] Sprite DefaultSprite;
    [SerializeField] Color DefaultColor = new Color(1, 1, 1, 0);
    [SerializeField] WeaponType AllowedType;
    
    public int Index;

    public event System.Action<ItemSlot> OnSlotClicked = delegate { };

    public void SetItem(Item item)
    {
        MyItem = item;
        Image.sprite = item.GetInventorySprite();
        Image.color = item.GetColor();
    }
    
    public void OnPointerUp(PointerEventData data)
    {
        OnSlotClicked(this);
    }

    public bool IsItemAllowed(Item item)
    {
        Weapon w = item as Weapon;
        if (w == null)
        {
            return false;
        }

        if (w.GetWeaponType() == AllowedType)
        {
            return true;
        }

        return false;
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
