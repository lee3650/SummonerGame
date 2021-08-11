using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSummonAttackState : RangedAttackState, IDamager, IControllableState
{
    List<Event> EventModifiers = new List<Event>();
    public void AddAttackModifier(Event e)
    {
        EventModifiers.Add(e);
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

    protected override void ActivateProjectile(Projectile p, IWielder wielder)
    {
        base.ActivateProjectile(p, wielder);
        foreach (Event e in EventModifiers)
        {
            p.AddAttackModifier(e);
        }
    }
}
