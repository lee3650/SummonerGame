using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPursuitState : MonoBehaviour, IState
{
    [SerializeField] TargetManager TargetManager;
    [SerializeField] AIStateMachine AIStateMachine;
    [SerializeField] MonoBehaviour ExitToState;
    [SerializeField] MovementController MovementController;
    [SerializeField] AIAttackManager AIAttackManager;

    public void EnterState()
    {
        if (TargetManager.Target == null)
        {
            throw new System.Exception("No target?");
        }
    }

    public void UpdateState()
    {
        if (TargetManager.Target.IsAlive())
        {
            if (!AIAttackManager.IsTargetInRange(TargetManager.Target))
            {
                TargetManager.MoveAtTarget();
            }
            
            TargetManager.LookAtTarget();
            AIAttackManager.TryAttack(TargetManager.Target); 
        }
        else
        {
            AIStateMachine.TransitionToState(ExitToState as IState);
        }
    }

    public void ExitState()
    {
    }
}
