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

    RotationController RotationController;

    Vector2 oldTargetPos; 

    private void Awake()
    {
        RotationController = GetComponent<RotationController>();
    }

    public void EnterState()
    {
        if (TargetManager.Target == null)
        {
            throw new System.Exception("No target?");
        }
        
        MovementController.SetPathfindGoal(TargetManager.Target.GetPosition());
        oldTargetPos = TargetManager.Target.GetPosition();
    }

    public void UpdateState()
    {
        if (TargetManager.Target.IsAlive())
        {
            if (ShouldMoveAtTarget())
            {
                TargetManager.MoveAtTarget();
                RotationController.FaceForward();
                
                if (Vector2.Distance(oldTargetPos, TargetManager.Target.GetPosition()) > 3) 
                {
                    oldTargetPos = TargetManager.Target.GetPosition();
                    MovementController.SetPathfindGoal(TargetManager.Target.GetPosition());
                }
            }
            
            //TargetManager.LookAtTarget();
            AIAttackManager.TryAttack(TargetManager.Target);
        }
        else
        {
            AIStateMachine.TransitionToState(ExitToState as IState);
        }
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
