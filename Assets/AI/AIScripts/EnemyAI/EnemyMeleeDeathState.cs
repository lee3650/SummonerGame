using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeDeathState : EnemyDeathState
{
    [SerializeField] GameObject MeleeWeapon;

    public override void VirtualEnterState()
    {
        if (MeleeWeapon != null)
        {
            MeleeWeapon.SetActive(false);
        }

        base.VirtualEnterState();
    }
}
