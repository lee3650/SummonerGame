using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllableAttackState : AIAttackState, IControllableState
{
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
}
