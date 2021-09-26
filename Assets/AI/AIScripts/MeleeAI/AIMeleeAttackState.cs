using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMeleeAttackState : AIAttackState
{
    [SerializeField] MeleeWeapon MeleeWeapon;
    [SerializeField] AIAttackManager AIAttackManager;

    public override void StartAttack()
    {
        MeleeWeapon.StartAttack(TargetManager.Target, AIAttackManager);
        base.StartAttack();
    }
}
