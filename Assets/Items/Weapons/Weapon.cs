using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item 
{
    //for now let's assume only the player can use items. 
    [SerializeField] private float ManaDrain; //we'll also assume you can only use mana 
    [SerializeField] private float AttackLength; 

    public float GetManaDrain()
    {
        return ManaDrain;
    }

    public float GetAttackLength()
    {
        return AttackLength;
    }

    public virtual void UseWeapon()
    {
    }

    public virtual WeaponType GetWeaponType()
    {
        return WeaponType.Summon;
    }
}
