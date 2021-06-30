﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAttackState : MonoBehaviour, IState
{
    [SerializeField] InventorySlotManager InventorySlotManager;
    [SerializeField] ItemSelection ItemSelection;
    [SerializeField] ManaManager ManaManager;
    [SerializeField] PlayerInput PlayerInput;
    [SerializeField] MovementController MovementController;

    [SerializeField] PlayerMoveState PlayerMoveState;
    [SerializeField] StateController StateController;

    float timer = 0f;

    public void EnterState()
    {
        timer = 0f;

        if (AttackConditionsMet())
        {
            SummonWeapon weapon = ItemSelection.SelectedItem as SummonWeapon;
            float attackDecrement = weapon.GetManaDrain();

            ManaManager.DecreaseMana(attackDecrement);
            if (weapon.ReduceMaxMana)
            {
                ManaManager.DecreaseMaxMana(attackDecrement);
            }

            weapon.UseWeapon();
            
            //play an animation, a sound, stuff like that
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
            if (ManaManager.IsManaMoreThanOrEqual(weapon.GetManaDrain()) && !InventorySlotManager.Active)
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

        timer += Time.deltaTime;

        if (timer >= (ItemSelection.SelectedItem as SummonWeapon).GetAttackLength())
        {
            StateController.TransitionToState(PlayerMoveState);
        }
    }

    public void ExitState()
    {

    }
}
