using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableSummon : Summon
{
    [SerializeField] TravelToPointState TravelToPointState;
    [SerializeField] StateController StateController;
    [SerializeField] TargetSearcher TargetSearcher;
    [SerializeField] HoldPointState HoldPointState;

    public virtual void GoToPoint(Vector2 point)
    {
        TravelToPointState.PointToTravelTo = point;
        StateController.TransitionToState(TravelToPointState);
    }

    public virtual void SetTarget(ITargetable target)
    {
        TargetSearcher.AssignTarget(target);
    }

    public override bool CanRefundMana()
    {
        return true; 
    }
    public virtual void HoldPoint(Vector2 point)
    {
        //so, basically we're not leaving the point until we die or until we get a command to move to a point 
        //hm. 

        //so, that does get a bit complex, right, because we need to assign the state to exit to, right. 
        
        HoldPointState.PointToHold = point;
        StateController.TransitionToState(HoldPointState);
    }
}
