using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPursuitState : MonoBehaviour, IState
{
    [SerializeField] protected TargetManager TargetManager;
    [SerializeField] AIStateMachine AIStateMachine;
    [SerializeField] MonoBehaviour ExitToState;
    [SerializeField] protected MovementController MovementController;
    [SerializeField] protected AIAttackManager AIAttackManager;

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
            if (ShouldMoveAtTarget())
            {
                TargetManager.MoveAtTarget();
            }
            
            TargetManager.LookAtTarget();
            AIAttackManager.TryAttack(TargetManager.Target);

            MiscUpdate(); 
        }
        else
        {
            AIStateMachine.TransitionToState(ExitToState as IState);
        }
    }
    
    public virtual void MiscUpdate()
    {

    }

    public virtual bool ShouldMoveAtTarget()
    {
        return !AIAttackManager.IsTargetInRange(TargetManager.Target);
    }

    public void SetExitState(MonoBehaviour exitState)
    {
        ExitToState = exitState;
    }

    public void ExitState()
    {
    }
}
