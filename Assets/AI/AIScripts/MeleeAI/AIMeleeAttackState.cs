using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMeleeAttackState : AIAttackState
{
    [SerializeField] MeleeWeapon MeleeWeapon;
    public override void StartAttack()
    {

        MeleeWeapon.StartAttack();
    }
}
