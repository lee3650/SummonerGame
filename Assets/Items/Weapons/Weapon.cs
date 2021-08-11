using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item 
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

    public abstract void UseWeapon(Vector2 mousePos);

    public virtual WeaponType GetWeaponType()
    {
        return WeaponType.Summon;
    }
}
