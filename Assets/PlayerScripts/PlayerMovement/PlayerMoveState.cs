using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : MonoBehaviour, IState
{
    [SerializeField] MovementController MovementController;
    [SerializeField] PlayerStateController PlayerStateController;
    [SerializeField] RotationController RotationController;
    [SerializeField] PlayerInput PlayerInput;

    [SerializeField] WaveSpawner WaveSpawner; 

    [SerializeField] ItemSelection ItemSelection;

    [SerializeField] PlayerDodgeState PlayerDodgeState;
    [SerializeField] PlayerAttackState PlayerAttackState;
    [SerializeField] float MouseMoveSensitivity;
    [SerializeField] float MoveDistThreshold;

    private Vector2 targetMousePos;

    //so, do we want to serialize the dodge state or just use get component? 
    //I feel like serialization is better, because it's slightly decoupled. 

    public void EnterState()
    {
        targetMousePos = PlayerInput.GetWorldMousePosition();
    }

    //So, okay. To click and drag the map around - we need to record where you clicked and maintain a delta from that position based on your mouse movements, right? 
    //or we could just add a new camera... let's try it this way, I guess.
    //... no... hm. Do we need a new camera? okay let's just try it this way. 

    public void UpdateState()
    {
        MoveWithMouse();

        Vector2 unitMoveDir = PlayerInput.GetUnitInputDirection();
        MovementController.MoveInDirection(unitMoveDir);

        RotationController.FacePoint(PlayerInput.GetWorldMousePosition());

        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerStateController.TransitionToState(PlayerDodgeState);
        }
         */

        if (ItemSelection.HasItem())
        {
            SummonWeapon sw = ItemSelection.SelectedItem as SummonWeapon;

            if (sw != null)
            {
                sw.UpdatePreview(SimplifiedAttackConditionsMet(sw), PlayerInput.GetWorldMousePosition());
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            PlayerStateController.TransitionToState(PlayerAttackState);
        }
    }

    public void MoveWithMouse()
    {
        if (Input.GetMouseButtonDown(1))
        {
            targetMousePos = PlayerInput.GetWorldMousePosition();
        }

        if (Input.GetMouseButton(1))
        {
            if (Vector2.Distance(targetMousePos, PlayerInput.GetWorldMousePosition()) > MoveDistThreshold)
            {
                transform.position = (Vector2)transform.position + MouseMoveSensitivity * (targetMousePos - PlayerInput.GetWorldMousePosition());
            }
        }
    }

    bool SimplifiedAttackConditionsMet(SummonWeapon weapon)
    {
        if (MapManager.IsMapInitialized())
        {
            if (MapManager.IsPointTraversable(PlayerInput.GetWorldMousePosition(), true))
            {
                if (!WaveSpawner.IsPointInSpawnRegion(VectorRounder.RoundVector(PlayerInput.GetWorldMousePosition())))
                {
                    if (weapon.CanUseWeapon(PlayerInput.GetWorldMousePosition()))
                    {
                        return true;
                    }
                }
            }
        }
        return false; 
    }
    public void ExitState()
    {
        //should we zero out our velocity? Probably not. 
    }
}
