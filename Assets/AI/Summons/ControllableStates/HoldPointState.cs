using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldPointState : MonoBehaviour, IControllableState
{
    [SerializeField] MovementController MovementController;
    [SerializeField] RotationController RotationController;
    [SerializeField] TargetManager TargetManager;
    [SerializeField] StateController StateController;
    [SerializeField] AIAttackManager AIAttackManager;

    PointToHoldManager PointToHoldManager;

    void Awake()
    {
        PointToHoldManager = GetComponent<PointToHoldManager>();
    }

    public void EnterState()
    {
        if (PointToHoldManager == null)
        {
            PointToHoldManager = GetComponent<PointToHoldManager>();
        }
        MovementController.SetPathfindGoal(PointToHoldManager.PointToHold);
    }

    public void UpdateState()
    {
        MovementController.MoveTowardPointWithRotation(PointToHoldManager.PointToHold);

        if (TargetManager.HasLivingTarget() && Vector2.Distance(transform.position, PointToHoldManager.PointToHold) < 2.5f)
        {
            AIAttackManager.TryAttack(TargetManager.Target);
        }
    }

    public void ExitState()
    {

    }

    public void HandleCommand(PlayerCommand command)
    {
        switch (command)
        {
            case HoldPointCommand hp:
                MovementController.SetPathfindGoal(hp.PointToHold);
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
