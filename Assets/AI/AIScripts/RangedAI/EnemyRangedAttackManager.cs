using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttackManager : AIAttackManager
{
    [SerializeField] SightChecker SightChecker;
    [SerializeField] float attackWidthDelta = 1.5f; 

    public override bool CanAttack(ITargetable CurrentTarget)
    {
        return base.CanAttack(CurrentTarget) && SightChecker.NoUnbreakableWallsInWay(CurrentTarget.GetPosition()) && IsTargetWithinDelta(CurrentTarget, transform.position, attackWidthDelta);
    }

    public static bool IsTargetWithinDelta(ITargetable target, Vector2 pos, float attackWidthDelta) //I'm aware this is poor organization 
    {
        float yDelta = Mathf.Abs(target.GetPosition().y - pos.y);

        if (yDelta <= attackWidthDelta)
        {
            return true; 
        }

        float xDelta = Mathf.Abs(target.GetPosition().x - pos.x);

        if (xDelta <= attackWidthDelta)
        {
            return true;
        }

        return false;
    }
}
