using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item, IPurchasable
{
    //for now let's assume only the player can use items. 
    [SerializeField] WeaponType WeaponType;
    [SerializeField] protected float ManaDrain; //we'll also assume you can only use mana 
    [SerializeField] private float AttackLength;
    [SerializeField] protected string WeaponDescription;
    [SerializeField] bool DeselectAfterAttacking = true;
    [SerializeField] bool RepeatAttack = false;

    public virtual float GetCost()
    {
        return GetManaDrain();
    }

    public virtual string GetDescription()
    {
        return FormatStringWidth(WeaponDescription); 
    }

    public bool AllowRepeatAttack()
    {
        return RepeatAttack;
    }

    protected string FormatStringWidth(string s)
    {
        return StringWidthFormatter.FormatStringToWidth(s, StringWidthFormatter.StandardWidth);
    }

    public virtual float GetRecurringCost()
    {
        return 0f; 
    }

    public virtual float GetManaDrain()
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

    public bool ShouldDeselectAfterAttacking()
    {
        return DeselectAfterAttacking;
    }

    public virtual WeaponType GetWeaponType()
    {
        return WeaponType;
    }
}
