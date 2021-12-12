using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotManager : MonoBehaviour
{
    [SerializeField] Inventory Inventory; //so, for now we're serializing the inventory it refers to. All these references may cause issues later. 

    [SerializeField] GameObject SlotMenu;

    [SerializeField] Transform Content;

    [SerializeField] InventoryRow InventoryRowPrefab;
    [SerializeField] InventorySlot InventorySlotPrefab;

    [SerializeField] int ItemsPerRow = 6; //can I make it const? 
    [SerializeField] int TotalRows = 0; //start with 0 rows. 

    [SerializeField] List<ItemType> InventoryItemTypes;
    [SerializeField] bool ControlHotbar = false;  

    private List<InventorySlot> InventorySlots = new List<InventorySlot>();
    private List<InventoryRow> InventoryRows = new List<InventoryRow>();

    private bool SwapMode = false;

    private InventorySlot SelectedSlot;

    [SerializeField] private List<ItemSlot> HotbarSlots;

    //really we need this to get setup by a 3rd party, kind of... 
    private void Awake()
    {
        //I guess on start? Sure, I guess. 
        Inventory.ItemDropped += Inventory_ItemDropped;
        Inventory.ItemPickedUp += Inventory_ItemPickedUp;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        SwapMode = false;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        SlotMenu.SetActive(false);
    }

    private void Inventory_ItemPickedUp(Item obj)
    {
        if (obj != null && InventoryItemTypes.Contains(obj.GetItemType()))
        {
            InventorySlot newSlot = Instantiate<InventorySlot>(InventorySlotPrefab);
            newSlot.SetItem(obj);
            newSlot.SetManager(this);
            InventorySlots.Add(newSlot);

            DisplayInventorySlots();
        }
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
        DestroyCurrentDisplay();

        CreateNewDisplay();

        UpdateHotbar();
    }

    void CreateNewDisplay()
    {
        int rowNum = Mathf.CeilToInt((float)InventorySlots.Count / (float)ItemsPerRow);

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
    
    void UpdateHotbar()
    {
        if (!ControlHotbar)
        {
            return;
        }

        bool[] slotTaken = new bool[HotbarSlots.Count];

        for (int i = 0; i < InventorySlots.Count; i++)
        {
            Item item = InventorySlots[i].GetItem();

            if (item != null)
            {
                for (int j = 0; j < HotbarSlots.Count; j++)
                {
                    if (HotbarSlots[j].IsItemAllowed(item) && !slotTaken[j]) //if it can hold that item and it doesn't have an item
                    {
                        HotbarSlots[j].SetItem(item);
                        slotTaken[j] = true;
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < HotbarSlots.Count; i++)
        {
            if (HotbarSlots[i].GetItem() == null)
            {
                HotbarSlots[i].ResetItem();
            }
        }

        /*
        for (int i = 0; i < HotbarSlots.Count; i++)
        {
            if (i < InventorySlots.Count)
            {
                HotbarSlots[i].SetItem(InventorySlots[i].GetItem());
            } else
            {
                HotbarSlots[i].ResetItem();
            }
        }
         */
    }
    
    public void ShowHotbarItemsOfTypes(List<BlueprintType> blueprintTypes)
    {
        foreach (ItemSlot slot in HotbarSlots)
        {
            if (slot.GetAllowedType() == WeaponType.Blueprint)
            {
                slot.ResetItem();
            }
        }

        int j = 0;
        for (int i = 0; i < HotbarSlots.Count; i++)
        {
            if (HotbarSlots[i].GetAllowedType() == WeaponType.Blueprint)
            {
                if (j < blueprintTypes.Count)
                {
                    HotbarSlots[i].SetAllowedBlueprint(blueprintTypes[j]);
                    j++;
                } else
                {
                    HotbarSlots[i].SetAllowedBlueprint(BlueprintType.None);
                }
            }
        }

        UpdateHotbar();
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

    public void EnterSwapMode()
    {
        SwapMode = true;
        SlotMenu.SetActive(false);
    }
    
    public void DropItem()
    {
        Inventory.DropItem(SelectedSlot.GetItem());
        SlotMenu.SetActive(false);
    }

    public void SlotSelected(InventorySlot slot)
    {
        //so, technically we want to get from the item what menu listings to show - i.e., equip, drink, etc... well, probably not drink, but equip, at least. 
        
        if (SwapMode)
        {
            Item oldItem = SelectedSlot.GetItem();
            SelectedSlot.SetItem(slot.GetItem());
            slot.SetItem(oldItem);
            SwapMode = false;

            UpdateHotbar();
        }
        else
        {
            SlotMenu.transform.position = slot.transform.position;
            SlotMenu.SetActive(true);
        }

        SelectedSlot = slot;
    }

    public bool Active
    {
        get
        {
            return gameObject.activeInHierarchy;
        }
    }
}
