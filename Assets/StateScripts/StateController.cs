using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    [SerializeField] Component StartingState;
    [SerializeField] bool InitOnAwake = false; 
    private IState CurrentState = null;

    private void Awake() //okay this could be bad lol 
    {
        TransitionToState(StartingState as IState);
    }

    public virtual void TransitionToState(IState newState)
    {
        if (CurrentState != null)
        {
            CurrentState.ExitState();
        }
        CurrentState = newState;
        CurrentState.EnterState();
    }

    public IState GetCurrentState()
    {
        return CurrentState;
    }

    public void Update()
    {
        CurrentState.UpdateState();
    }
}
