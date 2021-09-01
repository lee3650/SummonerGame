using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyRangedAttackManager : AIAttackManager
{
    [SerializeField] float attackWidthDelta = 1.5f; 

    public override bool CanAttack(ITargetable CurrentTarget)
    {
        bool baseOutput = base.CanAttack(CurrentTarget);
        bool deltaOutput = EnemyRangedAttackManager.IsTargetWithinDelta(CurrentTarget.GetPosition(), transform.position, attackWidthDelta);

        if (!baseOutput)
        {
            print("Could not attack because of base output!");
            return false;
        }

        if (!deltaOutput)
        {
            print("Could not attack because out delta output!");
            return false;
        }

        return true; 
    }

    public override bool IsCrossShaped()
    {
        return true;
    }

    public override float GetCrossDelta()
    {
        return AttackWidthDelta;
    }

    public float AttackWidthDelta
    {
        get
        {
            return attackWidthDelta;
        }
    }
}
