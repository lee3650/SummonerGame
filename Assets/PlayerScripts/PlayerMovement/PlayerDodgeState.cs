using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : MonoBehaviour, IState
{
    [SerializeField] MovementController MovementController; //so, should we do it like this? Apply force? 
    [SerializeField] PlayerStateController PlayerStateController;
    [SerializeField] RotationController RotationController;

    [SerializeField] PlayerMoveState PlayerMoveState;
    [SerializeField] PlayerInput PlayerInput;
    [SerializeField] float DodgeSpeed;
    [SerializeField] float DodgeLength;

    private float timer; 

    public void EnterState()
    {
        Vector2 dir = PlayerInput.GetUnitInputDirection();
        if (dir.magnitude < 0.2f)
        {
            float facingDir = RotationController.GetCurrentRotation() + 90f;

            float x = Mathf.Round(Mathf.Cos(facingDir * Mathf.Deg2Rad));
            float y = Mathf.Round(Mathf.Sin(facingDir * Mathf.Deg2Rad));

            dir = new Vector2(x, y);
        }

        //now, we need to divide dodge speed by the magnitude of the direction. 

        float actualSpeed = DodgeSpeed / dir.normalized.magnitude;

        MovementController.SetVelocity(dir, actualSpeed);

        RotationController.FaceDirection(dir);
        
        timer = DodgeLength;
    }

    public void UpdateState()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            PlayerStateController.TransitionToState(PlayerMoveState);
        }
    }

    public void ExitState()
    {

    }
}
