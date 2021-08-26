using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmReward : Reward
{
    [SerializeField] Charm MyCharm;
    [SerializeField] PlayerInventory PlayerInventory;
    //okay, that's one that could be static in the base class... 

    public override void ApplyReward()
    {
        Charm charm = Instantiate(MyCharm); 
        charm.GetComponent<Item>().OnDrop();
        PlayerInventory.PickUpItemAndTryToApplyCharm(charm.GetComponent<Item>());
    }
}
