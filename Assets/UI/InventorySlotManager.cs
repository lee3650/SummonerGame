﻿using System.Collections;
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

    private List<InventorySlot> InventorySlots = new List<InventorySlot>();
    private List<InventoryRow> InventoryRows = new List<InventoryRow>();

    private bool SwapMode = false;

    private InventorySlot SelectedSlot;

    [SerializeField] private List<ItemSlot> HotbarSlots;

    private void Start()
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
        InventorySlot newSlot = Instantiate<InventorySlot>(InventorySlotPrefab);
        newSlot.SetItem(obj);
        newSlot.SetManager(this);
        InventorySlots.Add(newSlot);

        DisplayInventorySlots();
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
