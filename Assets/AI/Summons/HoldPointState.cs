using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldPointState : MonoBehaviour, IState
{
    [SerializeField] AIPursuitState PursuitState;
    [SerializeField] MovementController MovementController;
    [SerializeField] RotationController RotationController;
    [SerializeField] TargetManager TargetManager;
    [SerializeField] StateController StateController;

    Vector2 pointToHold; 

    public void EnterState()
    {
        PursuitState.SetExitState(this);
        MovementController.SetPathfindGoal(pointToHold);
    }

    public void UpdateState()
    {
        if (Vector2.Distance(transform.position, PointToHold) > 0.75f)
        {
            MovementController.MonitorGoalAndFollowPath();
            RotationController.FaceForward();
        }

        if (TargetManager.HasLivingTarget() && Vector2.Distance(transform.position, PointToHold) < 2.5f)
        {
            StateController.TransitionToState(PursuitState);
        }
    }
    public void ExitState()
    {

    }

    public Vector2 PointToHold
    {
        get
        {
            return pointToHold; 
        }
        set
        {
            pointToHold = value;
        }
    }
}
