using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyRangedAttackManager : AIAttackManager
{
    [SerializeField] float attackWidthDelta = 1.5f; 

    public override bool CanAttack(ITargetable CurrentTarget)
    {
        return base.CanAttack(CurrentTarget) && EnemyRangedAttackManager.IsTargetWithinDelta(CurrentTarget.GetPosition(), transform.position, attackWidthDelta);
    }

    public float AttackWidthDelta
    {
        get
        {
            return attackWidthDelta;
        }
    }
}
