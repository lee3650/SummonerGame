using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    [SerializeField] Component StartingState;
    private IState CurrentState = null;

    void Start()
    {
        TransitionToState(StartingState as IState);
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
