using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISleepState : MonoBehaviour, IState
{
    [SerializeField] StateController StateController;
    [SerializeField] AIIdleState AIIdleState;
    [SerializeField] List<SpriteRenderer> RenderersToEnable = new List<SpriteRenderer>();
    AIEntity AIEntity;

    private void Awake()
    {
        AIEntity = GetComponent<AIEntity>();
    }

    public void WakeUp()
    {
        StateController.TransitionToState(AIIdleState);
    }

    public void EnterState()
    {
        SetRenderersEnabled(false);
    }
    public void UpdateState()
    {
        //so, there's no behavior, right. It's just waiting. 
    }

    void SetRenderersEnabled(bool val)
    {
        foreach (SpriteRenderer sr in RenderersToEnable)
        {
            sr.enabled = val;
        }
    }

    public void ExitState()
    {
        SetRenderersEnabled(true);
    }
}
