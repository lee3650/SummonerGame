using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine : StateController
{
    public override void TransitionToState(IState newState)
    {
        //print("To state: " + newState);
        base.TransitionToState(newState);
    }
}
