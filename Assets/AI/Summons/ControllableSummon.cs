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
    [SerializeField] PointToHoldManager PointToHoldManager;
    [SerializeField] DeactivatedState DeactivatedState;

    float originalHealAmount;

    bool Deactivated = false; 

    private void Start()
    {
        originalHealAmount = WaveHealAmt;
    }

    public void TransitionToRestState()
    {
        StateController.TransitionToState(RestState);
    }

    public void TransitionToDeactivatedState()
    {
        StateController.TransitionToState(DeactivatedState);
    }

    public void TransitionToHoldPointState()
    {
        StateController.TransitionToState(HoldPointState);
    }

    public void HandleCommand(PlayerCommand command)
    {
        switch (command)
        {
            case HoldPointCommand hp:
                HoldPoint(hp.PointToHold);
                break;
            case ToggleGuardModeCommand tg:
                ToggleGuardMode();
                break;
            case RestCommand rc: //since this changes the current state we don't want to do anything with it 
                break;

        }

        (StateController.GetCurrentState() as IControllableState).HandleCommand(command);
    }

    public override void OnWaveEnds()
    {
        if (GetSummoner().TryReduceMana(MaintenanceFee))
        {
            WaveHealAmt = originalHealAmount; //this is really not a good way to do it... idk. It's obviously not relying on abstraction, right? 
            if (Deactivated)
            {
                Deactivated = false; 
                HandleCommand(new ReactivateCommand());
            }
        }
        else
        {
            Deactivated = true; 
            WaveHealAmt = 0f;
            HandleCommand(new DeactivateCommand());
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
        PointToHoldManager.PointToHold = VectorRounder.RoundVector(point);
    }
}
