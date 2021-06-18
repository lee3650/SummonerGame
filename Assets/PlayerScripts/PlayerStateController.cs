using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    private IState CurrentState = null;
    [SerializeField] PlayerMoveState DefaultState;

    public void Start()
    {
        TransitionToState(DefaultState);
    }

    public void TransitionToState(IState newState)
    {
        if (CurrentState != null)
        {
            CurrentState.ExitState();
        }
        CurrentState = newState;
        CurrentState.EnterState();
    }
    
    public void Update()
    {
        CurrentState.UpdateState();       
    }
}
