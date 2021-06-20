using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggler : MonoBehaviour
{
    [SerializeField] GameObject InventoryParent;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        InventoryParent.SetActive(!InventoryParent.activeSelf);
    }
}
