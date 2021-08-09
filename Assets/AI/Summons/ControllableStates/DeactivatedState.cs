using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivatedState : MonoBehaviour, IControllableState
{
    [SerializeField] SpriteRenderer sr; 
    HoldPointState HoldPointState;
    MovementController MovementController;
    PointToHoldManager PointToHoldManager;
    StateController StateController;


    private void Awake()
    {
        StateController = GetComponent<StateController>();
        MovementController = GetComponent<MovementController>();
        PointToHoldManager = GetComponent<PointToHoldManager>();
        HoldPointState = GetComponent<HoldPointState>();
    }

    public void EnterState()
    {
        MovementController.SetPathfindGoal(PointToHoldManager.PointToHold);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.5f);
    }
    public void UpdateState()
    {
        MovementController.MoveTowardPointWithRotation(PointToHoldManager.PointToHold);
    }
    
    public void ExitState()
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
    }

    public void HandleCommand(PlayerCommand c)
    {
        switch (c)
        {
            case HoldPointCommand holdPoint:
                MovementController.SetPathfindGoal(holdPoint.PointToHold);
                break;
            case ReactivateCommand rc:
                StateController.TransitionToState(HoldPointState);
                break; 
        }
    }
}
