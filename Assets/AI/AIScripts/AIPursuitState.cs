using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPursuitState : MonoBehaviour, IState
{
    [SerializeField] TargetManager TargetManager;
    [SerializeField] AIStateMachine AIStateMachine;
    [SerializeField] AIIdleState IdleState;
    [SerializeField] MovementController MovementController;
    [SerializeField] AIAttackManager AIAttackManager;

    public void EnterState()
    {

    }

    public void UpdateState()
    {
        if (TargetManager.Target.IsAlive())
        {
            TargetManager.MoveAtTarget();
            TargetManager.LookAtTarget();
            AIAttackManager.TryAttack(TargetManager.Target); 
        }
        else
        {
            AIStateMachine.TransitionToState(IdleState);
        }
    }

    public void ExitState()
    {
    }
}
