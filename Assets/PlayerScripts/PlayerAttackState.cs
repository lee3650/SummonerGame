using System.Collections;
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

    [SerializeField] HealthManager HealthManager;

    [SerializeField] PlayerMoveState PlayerMoveState;
    [SerializeField] StateController StateController;

    [SerializeField] PlayerSummonController PlayerSummonController;

    float timer = 0f;

    int attackFrame = -1; 

    public void EnterState()
    {
        timer = 0f;

        if (AttackConditionsMet())
        {
            attackFrame = Time.frameCount;

            Weapon weapon = ItemSelection.SelectedItem as Weapon;
            float attackDecrement = weapon.GetManaDrain();

            if (ManaManager.TryDecreaseMana(attackDecrement) == false)
            {
                attackDecrement -= ManaManager.GetCurrent();
                ManaManager.DecreaseMana(ManaManager.GetCurrent());
                HealthManager.SubtractHealth(attackDecrement);
            }

            weapon.UseWeapon(PlayerInput.GetWorldMousePosition());
            
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
        Weapon weapon = ItemSelection.SelectedItem as Weapon;

        if (weapon != null)
        {
            if (!InventorySlotManager.Active)
            {
                if (HealthManager.GetCurrent() + ManaManager.GetCurrent() > weapon.GetManaDrain())
                {
                    if (MapManager.IsPointTraversable(PlayerInput.GetWorldMousePosition(), true))
                    {
                        //what is this? 
                        if (PlayerSummonController.IsMouseOverControllableSummon() == false)
                        {
                            if (PlayerSummonController.MouseOverUIComponent() == false)
                            {
                                if (PlayerSummonController.HadSelectionThisFrame() == false)
                                {
                                    return true;
                                } 
                            }
                        }
                    }
                }
            }
        }
        return false; 
    }

    public bool AttackedThisFrame()
    {
        return attackFrame == Time.frameCount; 
    }

    public void UpdateState()
    {
        Vector2 unitMoveDir = PlayerInput.GetUnitInputDirection();
        MovementController.MoveInDirection(unitMoveDir);

        timer += Time.deltaTime;

        if (timer >= (ItemSelection.SelectedItem as Weapon).GetAttackLength())
        {
            StateController.TransitionToState(PlayerMoveState);
        }
    }

    public void ExitState()
    {

    }
}
