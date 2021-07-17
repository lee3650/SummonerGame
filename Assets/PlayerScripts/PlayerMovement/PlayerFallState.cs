using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : MonoBehaviour, IState
{
    [SerializeField] StateController StateController;
    [SerializeField] HealthManager HealthManager;
    [SerializeField] PlayerMoveState PlayerMoveState;
    [SerializeField] MovementController MovementController;

    Vector2 initialScale; 

    float fallTimer = 0f;
    float fallLength = 0.5f; 

    public void EnterState()
    {
        HealthManager.SubtractHealth(5f);
        initialScale = transform.localScale;
        MovementController.SetVelocity(Vector2.zero, 0f);
        fallTimer = 0f; 
    }
    public void UpdateState()
    {
        fallTimer += Time.deltaTime;
        transform.localScale *= 0.95f; 
        if (fallTimer >= fallLength)
        {
            StateController.TransitionToState(PlayerMoveState);
        }
    }
    public void ExitState()
    {
        transform.localScale = initialScale;
        transform.position = MapManager.GetClosestValidTile(transform.position);
    }
}
