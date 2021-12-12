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
    [SerializeField] BlueprintType AllowedBlueprint = BlueprintType.Any; 
    
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

    public WeaponType GetAllowedType()
    {
        return AllowedType;
    }

    public void SetAllowedBlueprint(BlueprintType type)
    {
        AllowedBlueprint = type; 
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
            if (AllowedBlueprint == BlueprintType.Any)
            {
                return true;
            } else if (w.TryGetComponent<BlueprintWeapon>(out BlueprintWeapon bw))
            {
                return bw.GetBlueprintType() == AllowedBlueprint;
            }
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
