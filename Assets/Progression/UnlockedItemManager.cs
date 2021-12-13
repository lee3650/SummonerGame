using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedItemManager : MonoBehaviour
{
    [SerializeField] PlayerInventory PlayerInventory;
    [SerializeField] List<GameObject> DefaultItems;

    private void Start()
    {
        List<GameObject> items = ResearchManager.GetUnlockedItems();

        if (!MainMenuScript.TutorialMode)
        {
            items.AddRange(DefaultItems);

            foreach (GameObject item in items)
            {
                GameObject instance = Instantiate(item);
                PlayerInventory.TryToPickUpGameobject(instance);
            }
        }
    }
}
