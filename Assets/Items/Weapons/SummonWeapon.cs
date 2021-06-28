using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonWeapon : Weapon
{
    //so, we just override UseWeapon. 
    //What exactly is the point of the Weapon class? 
    public override void UseWeapon()
    {

    }

    public override WeaponType GetWeaponType()
    {
        return WeaponType.Summon;
    }
}
