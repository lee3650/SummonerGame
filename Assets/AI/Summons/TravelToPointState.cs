using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelToPointState : MonoBehaviour, IState
{
    [SerializeField] MovementController MovementController;
    [SerializeField] FollowSummonerState FollowSummonerState;
    [SerializeField] RotationController RotationController;
    StateController StateController;

    private void Awake()
    {
        StateController = GetComponent<StateController>();
    }

    public Vector2 PointToTravelTo
    {
        get;
        set;
    }

    public void EnterState()
    {

    }
    
    public void UpdateState()
    {
        if (Vector2.Distance(transform.position, PointToTravelTo) > 0.5f)
        {
            MovementController.MoveTowardPoint(PointToTravelTo);
            RotationController.FaceForward();
        } else
        {
            StateController.TransitionToState(FollowSummonerState as IState);
        }
    }

    public void ExitState()
    {

    }
}
