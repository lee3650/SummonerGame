using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item, IPurchasable
{
    //for now let's assume only the player can use items. 
    [SerializeField] private float ManaDrain; //we'll also assume you can only use mana 
    [SerializeField] private float AttackLength;
    [SerializeField] string WeaponDescription; 

    public virtual float GetCost()
    {
        return ManaDrain;
    }

    public virtual string GetDescription()
    {
        return StringWidthFormatter.FormatStringToWidth(WeaponDescription, StringWidthFormatter.StandardWidth); 
    }

    public virtual float GetRecurringCost()
    {
        return 0f; 
    }

    public float GetManaDrain()
    {
        return ManaDrain;
    }

    public virtual bool CanUseWeapon(Vector2 mousePos)
    {
        return true; 
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
