using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedPursuitState : AIPursuitState
{
    [SerializeField] SightChecker SightChecker;

    public override bool ShouldMoveAtTarget()
    {
        bool result = base.ShouldMoveAtTarget() || (!SightChecker.NoUnbreakableWallsInWay(TargetManager.Target.GetPosition())) || AIAttackManager.CanAttack(TargetManager.Target) == false;
        
        return result;
    }
}
