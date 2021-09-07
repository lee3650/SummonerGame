using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableMeleeDeathState : MeleeDeathState, IControllableState
{
    [SerializeField] PointToHoldManager PointToHoldManager;
    public override void VirtualEnterState()
    {
        TargetableEntitiesManager.RemoveSecondaryTarget(VectorRounder.RoundVectorToInt(PointToHoldManager.PointToHold));
        base.VirtualEnterState();
    }

    public void HandleCommand(PlayerCommand command)
    {

    }
}
