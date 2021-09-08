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
    [SerializeField] TargetSearcher TargetSearcher;

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

        MovementController.SetPathfindGoal(GetPathfindGoal());
        oldTargetPos = TargetManager.Target.GetPosition();
    }

    public void UpdateState()
    {
        if (ShouldKeepPursuingTarget())
        {
            if (ShouldMoveAtTarget())
            {
                TargetManager.MoveAtTarget();
                RotationController.FaceForward();

                if (ShouldRecalculatePathfinding())
                {
                    print("Recalculating pathfinding!");
                    oldTargetPos = TargetManager.Target.GetPosition();
                    MovementController.SetPathfindGoal(GetPathfindGoal());
                }
            }

            AIAttackManager.TryAttack(TargetManager.Target);
        }
        else
        {
            AIStateMachine.TransitionToState(ExitToState as IState);
        }
    }

    protected virtual Vector2 GetPathfindGoal()
    {
        return TargetManager.Target.GetPosition();
    }

    protected virtual bool ShouldKeepPursuingTarget()
    {
        return TargetManager.Target.IsAlive();
    }

    public virtual bool ShouldRecalculatePathfinding()
    {
        return (Vector2.Distance(oldTargetPos, TargetManager.Target.GetPosition()) > 3) || TargetManager.TargetChangedThisFrame();
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
