using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableSummon : Summon
{
    [SerializeField] StateController StateController;
    [SerializeField] TargetSearcher TargetSearcher;
    [SerializeField] HoldPointState HoldPointState;
    [SerializeField] SummonPursuitState PursuitState;
    [SerializeField] RestState RestState;
    [SerializeField] AIAttackManager AIAttackManager;
    [SerializeField] float MaintenanceFee;

    float originalHealAmount;

    private void Start()
    {
        originalHealAmount = WaveHealAmt;
    }
    public void EnterRestState()
    {
        StateController.TransitionToState(RestState);
    }

    public virtual void SetTarget(ITargetable target)
    {
        if (TargetSearcher != null)
        {
            TargetSearcher.AssignTarget(target);
        }
    }

    public void HandleCommand(PlayerCommand command)
    {
        //so, we just need to give it to the current state, is the plan. 
        //What I don't really like about that is I need to create a new state for every single 
        //existing generic state. Well, there's not really anything I can do about that... I don't see any nice elegant solution. 



    }

    public override void OnWaveEnds()
    {
        if (GetSummoner().TryReduceMana(MaintenanceFee))
        {
            WaveHealAmt = originalHealAmount; //this is really not a good way to do it... idk. It's obviously not relying on abstraction, right? 
            AIAttackManager.Activated = true; 
            //so, this really is not a good solution, but we're going to automatically set this to true every time we are paid
            //because this boolean is only for this specific case. 
        }
        else
        {
            WaveHealAmt = 0f;
            AIAttackManager.Activated = false; 
        }

        base.OnWaveEnds();
    }

    public bool CanBeSelected()
    {
        return !(StateController.GetCurrentState() is RestState);
    }

    public void ToggleGuardMode()
    {
        PursuitState.ToggleHoldingPoint();
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
        HoldPointState.PointToHold = VectorRounder.RoundVector(point);
        StateController.TransitionToState(HoldPointState);
        //I'm slowly eliminating all the advantages of state machines. 
    }
}
