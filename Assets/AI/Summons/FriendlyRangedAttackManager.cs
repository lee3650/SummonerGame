using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyRangedAttackManager : AIAttackManager
{
    //[SerializeField] float attackWidthDelta = 1.5f; 

    public override bool CanAttack(ITargetable CurrentTarget)
    {
        bool baseOutput = base.CanAttack(CurrentTarget);
        //bool deltaOutput = EnemyRangedAttackManager.IsTargetWithinDelta(CurrentTarget.GetPosition(), transform.position, attackWidthDelta);

        if (!baseOutput)
        {
            return false;
        }

        return true; 
    }

    public override bool IsCrossShaped()
    {
        return false;
    }

    public override float GetCrossDelta()
    {
        return 0f;
    }
}
