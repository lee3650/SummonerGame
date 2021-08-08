using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPursuitState : AIPursuitState, IControllableState
{
    [SerializeField] HoldPointState HoldPointState;
    [SerializeField] bool IgnoreHeldPoint = false; 

    public void ToggleHoldingPoint()
    {
        IgnoreHeldPoint = !IgnoreHeldPoint;
    }

    public void HandleCommand(PlayerCommand command)
    {

    }

    public override bool ShouldMoveAtTarget()
    {
        return base.ShouldMoveAtTarget() && (TargetManager.DistanceFromTargetToPoint(HoldPointState.PointToHold) < 1.5f || IgnoreHeldPoint); 
    }
}
