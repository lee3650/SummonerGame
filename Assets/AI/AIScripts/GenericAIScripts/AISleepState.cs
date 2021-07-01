using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISleepState : MonoBehaviour, IState
{
    [SerializeField] StateController StateController;
    [SerializeField] AIIdleState AIIdleState;
    AIEntity AIEntity;

    private void Awake()
    {
        AIEntity = GetComponent<AIEntity>();
    }

    public void WakeUp()
    {
        if (AIEntity.IsAlive())
        {
            StateController.TransitionToState(AIIdleState);
        }
    }

    public void EnterState()
    {
    }
    public void UpdateState()
    {
        //so, there's no behavior, right. It's just waiting. 
    }
    public void ExitState()
    {

    }
}
