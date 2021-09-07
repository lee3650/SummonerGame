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
    [SerializeField] bool RecalculatePathIfTargetInWay = false;

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

        TryToSetTargetToAdjacent();

        MovementController.SetPathfindGoal(GetPathfindGoal());
        bool reset = TryToSetTargetToTargetInWay();

        if (reset && RecalculatePathIfTargetInWay)
        {
            MovementController.SetPathfindGoal(GetPathfindGoal());
        }
        oldTargetPos = TargetManager.Target.GetPosition();
    }

    protected void TryToSetTargetToAdjacent()
    {
        ITargetable newTarget = TargetableEntitiesManager.GetTargetableAdjacentTo(VectorRounder.RoundVectorToInt(transform.position));
        if (newTarget != null)
        {
            TargetSearcher.AssignTarget(newTarget); //hm. This might be a bit messed up for going through walls, but whatever for now 
        }
    }

    //this is going to have to change slightly for ranged pursuit state, so 
    protected bool TryToSetTargetToTargetInWay()
    {
        SearchNode node = MovementController.GetPathfindPath();
        
        while (node.ParentNode != null)
        {
            ITargetable newTarget = TargetableEntitiesManager.GetTargetableAdjacentTo(new Vector2Int(node.x, node.y));
            if (newTarget != null)
            {
                TargetSearcher.AssignTarget(newTarget);
                return true; 
            }
            node = node.ParentNode;
        }

        return false; 
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
