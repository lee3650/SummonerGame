using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelection : MonoBehaviour
{
    [SerializeField] ItemSlot[] ItemSlots;

    [SerializeField] Transform SelectionImage;

    [SerializeField] PlayerEntity PlayerEntity;

    [SerializeField] HotbarSelectorScript HotbarSelector; 

    private int ItemSlotsLength;

    public event Action SelectedItemChanged = delegate { };

    public Item SelectedItem
    {
        get;
        set;
    }

    int selectedIndex = -1;

    public int SelectedIndex
    {
        get
        {
            return selectedIndex;
        }
    }

    private void Awake()
    {
        AssignSlotIndexes();
        ItemSlotsLength = ItemSlots.Length;
        SubscribeToSlotEvents();
        gameObject.SetActive(false);
        HotbarSelector.HotbarSelectorClicked += HotbarSelectorClicked;
    }

    private void HotbarSelectorClicked()
    {
        print("Hotbar selector clicked!");
        DeselectItem();
    }

    private void AssignSlotIndexes()
    {
        for (int i = 0; i < ItemSlots.Length; i++)
        {
            ItemSlots[i].Index = i;
        }
    }

    private void SubscribeToSlotEvents()
    {
        foreach (ItemSlot s in ItemSlots)
        {
            s.OnSlotClicked += OnSlotClicked;
        }
    }

    private void OnSlotClicked(ItemSlot slot)
    {
        print("changing selection!");

        if (slot != null)
        {
            print("changing selection!");
            ChangeSelection(slot.Index);
        }
    }

    public bool HasItem()
    {
        if (SelectedItem != null)
        {
            return true;
        }
        return false; 
    }

    public void DeselectItem()
    {
        if (!PlayerEntity.CanChangeSelectedItem())
        {
            return;
        }

        if (SelectedItem != null)
        {
            SelectedItem.OnDeselection();
        }

        SelectionImage.gameObject.SetActive(false);

        selectedIndex = -1;

        SelectedItem = null; 
    
        SelectedItemChanged();
    }

    private void ChangeSelection(int newSelect)
    {
        if (!PlayerEntity.CanChangeSelectedItem())
        {
            return; 
        }

        print("new select: " + newSelect);
        print("Old select: " + selectedIndex);

        if (newSelect == selectedIndex)
        {
            print("Deselecting item!");
            DeselectItem();
            //ChangeSelection((newSelect + ItemSlotsLength / 2) % ItemSlotsLength);
            return; 
        }

        if (SelectedItem != null)
        {
            SelectedItem.OnDeselection();
        }

        SelectionImage.transform.SetParent(ItemSlots[newSelect].transform, false);
        SelectionImage.localPosition = Vector2.zero;
        SelectionImage.gameObject.SetActive(true);

        selectedIndex = newSelect;

        SelectedItem = ItemSlots[newSelect].GetItem();
        
        if (SelectedItem != null)
        {
            SelectedItem.OnSelection();
        }

        SelectedItemChanged();
    }
}