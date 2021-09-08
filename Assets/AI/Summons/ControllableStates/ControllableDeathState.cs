using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableDeathState : AIDeathState, IControllableState
{
    [SerializeField] PointToHoldManager PointToHoldManager;

    public void HandleCommand(PlayerCommand command)
    {

    }
}
