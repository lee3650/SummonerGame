using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableSummon : Summon, IControllableSummon, IRecurringCost
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
    [SerializeField] AIAttackState AttackState;

    [SerializeField] List<string> StatString; 

    float originalHealAmount;

    bool Deactivated = false;

    private void Start()
    {
        originalHealAmount = WaveHealAmt;
    }

    public string GetStatString()
    {
        string result = "";

        foreach (string s in StatString)
        {
            result += s + "\n"; 
        }
        return result;
    }

    public void TransitionToRestState()
    {
        StateController.TransitionToState(RestState);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void TransitionToDeactivatedState()
    {
        StateController.TransitionToState(DeactivatedState);
    }

    public void TransitionToHoldPointState()
    {
        StateController.TransitionToState(HoldPointState);
    }

    public virtual void HandleCommand(PlayerCommand command)
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
            case UpgradeCommand uc:
                UpgradeSummon(uc);
                break;
            case SellCommand sc:
                //we basically just kill ourselves here. 
                SellSummon();
                break; 
        }

        (StateController.GetCurrentState() as IControllableState).HandleCommand(command);
    }

    /*
     int adjacentImpassableTiles = MapManager.GetNumOfAdjacentImpassableTiles(Mathf.RoundToInt(PointToHoldManager.PointToHold.x), Mathf.RoundToInt(PointToHoldManager.PointToHold.y));

                float totalModifier = 1 - (AttackSpeedModifierPerTile * adjacentImpassableTiles);
                AttackState.MultiplyAttackLength(totalModifier);
                print("Attack modifier: " + totalModifier);

                float healthModifier = 1 + (HealthModifierPerTile * adjacentImpassableTiles);
                HealthManager.HealToNewMax(healthModifier);
                print("Health modifier: " + healthModifier);
         
         */

    public bool CanBeSold()
    {
        return true; 
    }

    void SellSummon()
    {
        HealthManager.SubtractHealth(100000);
        gameObject.SetActive(false); //I need to stop doing this lol. 
        print("Todo - remove these inactive gameobjects. ");
    }

    void UpgradeSummon(UpgradeCommand uc)
    {
        HealthManager.SubtractHealth(100000f);
        gameObject.SetActive(false);
        SummonWeapon.UpgradeSummon(uc.UpgradePath, transform.position, GetSummoner(), GetComponent<Sellable>());
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
        return HealthManager.IsAlive();
    }

    public void ToggleGuardMode()
    {
        PursuitState.ToggleHoldingPoint();
    }

    public override bool CanRefundMana()
    {
        return true;
    }

    public float GetRecurringCost()
    {
        return MaintenanceFee;
    }

    public virtual void HoldPoint(Vector2 point)
    {
        PointToHoldManager.PointToHold = VectorRounder.RoundVector(point);
    }
}
