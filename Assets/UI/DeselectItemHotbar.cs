using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeselectItemHotbar : MonoBehaviour
{
    [SerializeField] ItemSelection Hotbar;
    [SerializeField] AnimateInAndOut AnimateInAndOut;
    int minIndex;
    int maxIndex;

    private void Awake()
    {
        AnimateInAndOut.AnimatingOut += TryDeselect;
        ItemSlot[] slots = GetComponentsInChildren<ItemSlot>();
        minIndex = findMinIndex(slots);
        maxIndex = findMaxIndex(slots);
    }

    public void TryDeselect()
    {
        int sel = Hotbar.SelectedIndex;
        if (sel >= minIndex && sel <= maxIndex)
        {
            Hotbar.DeselectItem();
        }
    }

    private int findMinIndex(ItemSlot[] slots)
    {
        int min = 1000; 
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Index < min)
            {
                min = slots[i].Index;
            }
        }

        return min;
    }

    private int findMaxIndex(ItemSlot[] slots)
    {
        int max = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Index > max)
            {
                max = slots[i].Index;
            }
        }

        return max;
    }
}
