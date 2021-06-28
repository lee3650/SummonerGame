using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : MonoBehaviour, IState
{
    [SerializeField] ItemSelection ItemSelection;
    [SerializeField] ManaManager ManaManager;
    [SerializeField] PlayerInput PlayerInput;
    [SerializeField] MovementController MovementController;

    [SerializeField] PlayerMoveState PlayerMoveState;
    [SerializeField] StateController StateController;

    public void EnterState()
    {
        if (AttackConditionsMet())
        {
            SummonWeapon weapon = ItemSelection.SelectedItem as SummonWeapon;
            float attackDecrement = weapon.GetManaDrain();



        } else
        {
            StateController.TransitionToState(PlayerMoveState);
        }
    }

    private bool AttackConditionsMet()
    {
        //so, what conditions do we have here? 
        //if the selected item is a summon weapon
        //if we have enough mana 
        SummonWeapon weapon = ItemSelection.SelectedItem as SummonWeapon;

        if (weapon != null)
        {
            if (ManaManager.IsManaMoreThanOrEqual(weapon.GetManaDrain()))
            {
                return true;
            }
        }
        return false; 
    }

    public void UpdateState()
    {
        Vector2 unitMoveDir = PlayerInput.GetUnitInputDirection();
        MovementController.MoveInDirection(unitMoveDir);


    }
    public void ExitState()
    {

    }
}
