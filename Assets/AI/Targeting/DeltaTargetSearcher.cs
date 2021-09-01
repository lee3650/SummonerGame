using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeltaTargetSearcher : TargetSearcher
{
    [SerializeField] FriendlyRangedAttackManager FriendlyRangedAttackManager;

    protected override bool ShouldAddCandidate(ILivingEntity e)
    {
        return base.ShouldAddCandidate(e) && EnemyRangedAttackManager.IsTargetWithinDelta(e.GetPosition(), transform.position, FriendlyRangedAttackManager.AttackWidthDelta);
    }
}
