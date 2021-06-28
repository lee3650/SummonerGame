using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item 
{
    //for now let's assume only the player can use items. 
    [SerializeField] private float ManaDrain; //we'll also assume you can only use mana 

    public float GetManaDrain()
    {
        return ManaDrain;
    }

    public override void OnPickup(Transform collector)
    {
        base.OnPickup(collector);
    }

    public virtual void UseWeapon()
    {
    }
}
