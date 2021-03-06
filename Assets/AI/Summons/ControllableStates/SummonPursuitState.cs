using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonPursuitState : AIPursuitState, IControllableState
{
    [SerializeField] bool IgnoreHeldPoint = false;
    [SerializeField] PointToHoldManager PointToHoldManager;

    public void ToggleHoldingPoint()
    {
        IgnoreHeldPoint = !IgnoreHeldPoint;
    }

    public void HandleCommand(PlayerCommand command)
    {
        switch (command)
        {
            case HoldPointCommand hp:
                GetComponent<ControllableSummon>().TransitionToHoldPointState();
                break;
            case ToggleGuardModeCommand tg:
                break;
            case RestCommand rc:
                GetComponent<ControllableSummon>().TransitionToRestState();
                break;
            case DeactivateCommand dc:
                GetComponent<ControllableSummon>().TransitionToDeactivatedState();
                break;
        }
    }

    protected override bool ShouldKeepPursuingTarget()
    {
        return base.ShouldKeepPursuingTarget() && InRange(); 
    }

    public override bool ShouldMoveAtTarget()
    {
        return base.ShouldMoveAtTarget() && InRange();
    }

    bool InRange()
    {
        return (TargetManager.DistanceFromTargetToPoint(PointToHoldManager.PointToHold) < 1.5f || IgnoreHeldPoint);
    }
}