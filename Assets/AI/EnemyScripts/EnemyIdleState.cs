using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : MonoBehaviour, IState
{
    [SerializeField] TargetManager TargetManager;
    [SerializeField] StateController StateController;
    [SerializeField] EnemyPursuitState PursuitState;

    public void EnterState()
    {

    }

    public void UpdateState()
    {
        if (TargetManager.HasTarget())
        {
            StateController.TransitionToState(PursuitState);
        }
    }
    
    public void ExitState()
    {
    }
}
