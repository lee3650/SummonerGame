using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : MonoBehaviour, IState
{
    [SerializeField] MovementController MovementController;
    [SerializeField] PlayerStateController PlayerStateController;
    [SerializeField] RotationController RotationController;
    [SerializeField] PlayerInput PlayerInput;

    [SerializeField] PlayerDodgeState PlayerDodgeState;
    [SerializeField] PlayerAttackState PlayerAttackState;

    //so, do we want to serialize the dodge state or just use get component? 
    //I feel like serialization is better, because it's slightly decoupled. 

    public void EnterState()
    {

    }

    public void UpdateState()
    {
        Vector2 unitMoveDir = PlayerInput.GetUnitInputDirection();
        MovementController.MoveInDirection(unitMoveDir);

        RotationController.FacePoint(PlayerInput.GetWorldMousePosition());
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerStateController.TransitionToState(PlayerDodgeState);
        }

        if (Input.GetMouseButtonDown(0))
        {
            PlayerStateController.TransitionToState(PlayerAttackState);
        }
    }
    
    public void ExitState()
    {
        //should we zero out our velocity? Probably not. 
    }
}
