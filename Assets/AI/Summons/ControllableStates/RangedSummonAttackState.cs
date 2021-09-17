using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSummonAttackState : RangedAttackState, IDamager, IControllableState
{
    const float AttackSpeedModifierPerTile = 0.05f; 

    [SerializeField] PointToHoldManager PointToHoldManager;
    List<Event> EventModifiers = new List<Event>();
    
    public void AddAttackModifier(Event e)
    {
        EventModifiers.Add(e);
    }

    public override void StartAttack()
    {
        int adjacentImpassableTiles = MapManager.GetNumOfAdjacentImpassableTiles(Mathf.RoundToInt(PointToHoldManager.PointToHold.x), Mathf.RoundToInt(PointToHoldManager.PointToHold.y));
        float speedModifier = 1 - (AttackSpeedModifierPerTile * adjacentImpassableTiles);
        MultiplyAttackLength(speedModifier); //so, do note that this will undo any other applied speed increases, so. 
        print("Attack length");
        base.StartAttack();
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
