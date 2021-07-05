using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPursuitState : AIPursuitState
{
    [SerializeField] HoldPointState HoldPointState;

    public override void MiscUpdate()
    {
        if (Vector2.Distance(transform.position, HoldPointState.PointToHold) > 2f)
        {
            MovementController.MoveTowardPoint(HoldPointState.PointToHold);
        }
    }
    
    public override bool ShouldMoveAtTarget()
    {
        return base.ShouldMoveAtTarget() && TargetManager.DistanceFromTargetToPoint(HoldPointState.PointToHold) < 1.5f; 
    }
}
