using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryToggler : MonoBehaviour
{
    [SerializeField] List<InventorySlotManager> InventorySlotManagers;
    [SerializeField] List<GameObject> ParallelToggleUI; 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        for (int i = 0; i < InventorySlotManagers.Count; i++)
        {
            InventorySlotManager m = InventorySlotManagers[i];
            if (m.Active)
            {
                m.Hide();
                ParallelToggleUI[i].SetActive(false);
            }
            else
            {
                m.Show();
                ParallelToggleUI[i].SetActive(true);
            }
        }
    }
}
