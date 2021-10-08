﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelection : MonoBehaviour
{
    [SerializeField] ItemSlot[] ItemSlots;

    [SerializeField] Transform SelectionImage;

    [SerializeField] PlayerEntity PlayerEntity;

    private int ItemSlotsLength;

    private const int ZeroIndex = (int)KeyCode.Alpha1;

    public event Action SelectedItemChanged = delegate { };

    public Item SelectedItem
    {
        get;
        set;
    }

    int selectedIndex = -1;

    private void Awake()
    {
        ItemSlotsLength = ItemSlots.Length;
    }
    
    public bool HasItem()
    {
        if (SelectedItem != null)
        {
            return true;
        }
        return false; 
    }

    private void Update()
    {
        for (int i = 0; i < ItemSlotsLength; i++)
        {
            if (Input.GetKeyDown((KeyCode)(ZeroIndex + i)))
            {
                ChangeSelection(i);
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            if (selectedIndex != -1)
            {
                ChangeSelection(selectedIndex);
            }
        }

        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0)
        {
            int newIndex;
            
            if (Input.mouseScrollDelta.y > 0)
            {
                newIndex = selectedIndex - 1;
                if (newIndex < 0)
                {
                    newIndex = ItemSlotsLength - 1;
                }
            }
            else
            {
                newIndex = selectedIndex + 1;
                if (newIndex >= ItemSlotsLength)
                {
                    newIndex = 0;
                }
            }
            
            ChangeSelection(newIndex);
        }
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

        if (newSelect == selectedIndex)
        {
            //DeselectItem();
            ChangeSelection((newSelect + ItemSlotsLength / 2) % ItemSlotsLength);
            return; 
        }

        if (SelectedItem != null)
        {
            SelectedItem.OnDeselection();
        }

        SelectionImage.position = ItemSlots[newSelect].GetPosition();
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