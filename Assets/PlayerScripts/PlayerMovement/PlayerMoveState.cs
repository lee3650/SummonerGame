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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //um... well, we need to know if something is selected here. 
            //we only want to do one of those things, right. 
            //eh, actually who cares. 

            ItemSelection.DeselectItem();
        }

        if (Input.GetMouseButtonDown(0))
        {
            PlayerStateController.TransitionToState(PlayerAttackState);
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
