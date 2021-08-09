using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestState : MonoBehaviour, IControllableState, IWaveNotifier
{
    [SerializeField] HoldPointState HoldPointState;
    [SerializeField] StateController StateController;
    [SerializeField] HealthManager HealthManager;
    [SerializeField] MovementController MovementController;

    PointToHoldManager PointToHoldManager;

    void Awake()
    {
        PointToHoldManager = GetComponent<PointToHoldManager>();
    }

    public void EnterState()
    {
        WaveSpawner.NotifyWhenWaveEnds(this);
        MovementController.SetPathfindGoal(PointToHoldManager.PointToHold);
    }

    public void UpdateState()
    {
        MovementController.MoveTowardPointWithRotation(PointToHoldManager.PointToHold);
    }

    public void HandleCommand(PlayerCommand command)
    {
        switch (command)
        {
            case HoldPointCommand hp:
                MovementController.SetPathfindGoal(hp.PointToHold);
                break;
            case DeactivateCommand dc:
                GetComponent<ControllableSummon>().TransitionToDeactivatedState();
                break;
        }
    }

    public void OnWaveEnds()
    {
        StateController.TransitionToState(HoldPointState);
        HealthManager.HealToFull();
    }

    public void ExitState()
    {
        WaveSpawner.StopNotifyingWhenWaveEnds(this);
    }
}
