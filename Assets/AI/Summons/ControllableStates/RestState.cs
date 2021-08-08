using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestState : MonoBehaviour, IControllableState, IWaveNotifier
{
    [SerializeField] HoldPointState HoldPointState;
    [SerializeField] StateController StateController;
    [SerializeField] HealthManager HealthManager; 

    public void EnterState()
    {
        WaveSpawner.NotifyWhenWaveEnds(this);
    }
    public void UpdateState()
    {

    }

    public void HandleCommand(PlayerCommand command)
    {

    }

    public void OnWaveEnds()
    {
        StateController.TransitionToState(HoldPointState);
        //heal to full
        HealthManager.HealToFull();
    }

    public void ExitState()
    {
        WaveSpawner.StopNotifyingWhenWaveEnds(this);
    }
}
