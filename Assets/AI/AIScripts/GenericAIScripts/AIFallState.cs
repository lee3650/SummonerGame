using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFallState : MonoBehaviour, IState
{
    [SerializeField] HealthManager HealthManager;
    [SerializeField] SpriteRenderer sr; 
    [SerializeField] StateController StateController; 
    [SerializeField] AIDeathState AIDeathState;
    float FallTime = 0.25f;

    float timer = 0f; 

    public void EnterState()
    {
        timer = 0f; 
    }

    public void UpdateState()
    {
        transform.localScale *= 0.9f;
        timer += Time.deltaTime; 
        if (timer > FallTime)
        {
            HealthManager.SubtractHealth(10000f);
        }
    }

    public void ExitState()
    {
        sr.enabled = false; 
    }
}
