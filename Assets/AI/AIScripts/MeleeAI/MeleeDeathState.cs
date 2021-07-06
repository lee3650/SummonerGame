using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDeathState : AIDeathState
{
    [SerializeField] GameObject MeleeWeapon; 

    public override void VirtualEnterState()
    {
        if (MeleeWeapon != null)
        {
            MeleeWeapon.SetActive(false);
        }
    }
}
