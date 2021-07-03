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

    public void EnterState()
    {
        PursuitState.SetExitState(this);
    }

    public void UpdateState()
    {
        if (Vector2.Distance(transform.position, PointToHold) > 0.5f)
        {
            MovementController.MoveTowardPoint(PointToHold);
            RotationController.FaceForward();
        }

        if (TargetManager.HasTarget())
        {
            StateController.TransitionToState(PursuitState);
        }
    }
    public void ExitState()
    {

    }

    public Vector2 PointToHold
    {
        get;
        set;
    }
}
