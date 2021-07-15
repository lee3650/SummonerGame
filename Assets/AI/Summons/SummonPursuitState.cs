using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPursuitState : AIPursuitState
{
    [SerializeField] HoldPointState HoldPointState;
    
    public override bool ShouldMoveAtTarget()
    {
        return base.ShouldMoveAtTarget() && TargetManager.DistanceFromTargetToPoint(HoldPointState.PointToHold) < 1.5f; 
    }
}
