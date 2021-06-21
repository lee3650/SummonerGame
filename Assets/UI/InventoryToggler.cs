using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggler : MonoBehaviour
{
    [SerializeField] InventorySlotManager InventorySlotManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        if (InventorySlotManager.Active)
        {
            InventorySlotManager.Hide();
        } 
        else
        {
            InventorySlotManager.Show();
        }
    }
}
