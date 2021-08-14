using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachine : StateController
{
    //what is the point of this lol 
    public override void TransitionToState(IState newState)
    {
        //print("To state: " + newState); 
        base.TransitionToState(newState);
    }
}
