using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttackManager : AIAttackManager
{
    [SerializeField] SightChecker SightChecker;
    protected override bool CanAttack(ITargetable CurrentTarget)
    {
        return base.CanAttack(CurrentTarget) && SightChecker.CanSeePathToTarget(CurrentTarget.GetPosition());
    }
}
