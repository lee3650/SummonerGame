using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIdleState : MonoBehaviour, IState
{
    [SerializeField] TargetManager TargetManager;
    [SerializeField] StateController StateController;
    [SerializeField] AIPursuitState PursuitState;

    public void EnterState()
    {

    }

    public void UpdateState()
    {
        if (TargetManager.HasLivingTarget())
        {
            StateController.TransitionToState(PursuitState);
        }
    }
    
    public void ExitState()
    {
    }
}
