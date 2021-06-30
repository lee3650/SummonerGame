using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableSummon : Summon
{
    [SerializeField] TravelToPointState TravelToPointState;
    [SerializeField] StateController StateController;
    [SerializeField] TargetSearcher TargetSearcher;

    public virtual void GoToPoint(Vector2 point)
    {
        TravelToPointState.PointToTravelTo = point;
        StateController.TransitionToState(TravelToPointState);
    }

    public virtual void SetTarget(ITargetable target)
    {
        TargetSearcher.AssignTarget(target);
    }
}
