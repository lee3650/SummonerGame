using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttackManager : AIAttackManager
{
    [SerializeField] SightChecker SightChecker;
    protected override bool CanAttack(ITargetable CurrentTarget)
    {
        return base.CanAttack(CurrentTarget) && (!CurrentTarget.RequireLineOfSight() || SightChecker.CanSeePathToTarget(CurrentTarget.GetPosition()));
    }
}
