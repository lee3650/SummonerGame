using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : Inventory
{
    [SerializeField] private float ReachRadius;
    [SerializeField] Summoner MySummoner;

    [SerializeField] List<GameObject> StartingItems;

    private void Start() //we don't want this in awake because the other inventory stuff needs to get setup first I believe
    {
        foreach (GameObject g in StartingItems)
        {
            Item item;
            if (g.TryGetComponent<Item>(out item))
            {
                if (item.CanBePickedUp())
                {
                    PickUpItem(item);
                    if (item.GetItemType() == ItemType.Charm)
                    {
                        MySummoner.AddCharm(item.GetComponent<Charm>());
                    }
                }
            }
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickUpNearbyItems();
        }
    }
    
    private void PickUpNearbyItems()
    {
        Collider2D[] items = Physics2D.OverlapCircleAll(transform.position, ReachRadius);

        print("items found: " + items.Length);

        foreach (Collider2D col in items)
        {
            Item item;
            if (col.TryGetComponent<Item>(out item))
            {
                if (item.CanBePickedUp())
                {
                    PickUpItem(item);
                    if (item.GetItemType() == ItemType.Charm)
                    {
                        MySummoner.AddCharm(item.GetComponent<Charm>());
                    }
                }
            }
        }
    }
}
