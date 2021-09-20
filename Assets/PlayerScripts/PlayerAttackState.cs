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

    [SerializeField] WaveSpawner WaveSpawner; //okay now this is sus 

    [SerializeField] PlayerMoveState PlayerMoveState;
    [SerializeField] StateController StateController;

    [SerializeField] PlayerSummonController PlayerSummonController;
    [SerializeField] Summoner Summoner;

    float timer = 0f;

    bool deselectAfter; 

    int attackFrame = -1; 

    public void EnterState()
    {
        timer = 0f;

        if (AttackConditionsMet())
        {
            attackFrame = Time.frameCount;

            Weapon weapon = ItemSelection.SelectedItem as Weapon;
            float attackDecrement = weapon.GetManaDrain();

            //so, this method should never get called in theory 
            if (ManaManager.TryDecreaseMana(attackDecrement) == false)
            {
                SubtractManaAndHealth(attackDecrement);
            }

            weapon.UseWeapon(PlayerInput.GetWorldMousePosition());

            deselectAfter = weapon.ShouldDeselectAfterAttacking();

            //play an animation, a sound, stuff like that

            Summoner.OnFinancialsChanged();
        } else
        {
            StateController.TransitionToState(PlayerMoveState);
        }
    }

    void SubtractManaAndHealth(float attackDecrement)
    {
        attackDecrement -= ManaManager.GetCurrent();
        ManaManager.DecreaseMana(ManaManager.GetCurrent());
        HealthManager.SubtractHealth(attackDecrement);
    }

    public bool AttackConditionsMet()
    {
        Weapon weapon = ItemSelection.SelectedItem as Weapon;

        if (!InventoryIsValid(weapon))
        {
            return false; 
        }

        if (!SufficientEnergy(weapon))
        {
            return false;
        }

        if (!IsPositionSpawnable())
        {
            return false;
        }

        if (PlayerIsSelecting())
        {
            return false;
        }

        if (!IsWeaponUseable(weapon))
        {
            return false;
        }

        return true;
    }

    public bool InventoryIsValid(Weapon weapon)
    {
        if (weapon == null)
        {
            return false;
        }

        if (InventorySlotManager.Active)
        {
            return false;
        }

        return true; 
    }

    public bool IsWeaponUseable(Weapon weapon)
    {
        return weapon.CanUseWeapon(PlayerInput.GetWorldMousePosition());
    }

    bool SufficientEnergy(Weapon weapon)
    {
        return ManaManager.GetCurrent() >= weapon.GetManaDrain(); //HealthManager.GetCurrent() + 
    }

    public bool IsPositionSpawnable()
    {
        return MapManager.IsPointTraversable(PlayerInput.GetWorldMousePosition(), true) && !WaveSpawner.IsPointInSpawnRegion(VectorRounder.RoundVector(PlayerInput.GetWorldMousePosition()));
    }

    bool PlayerIsSelecting()
    {
        if (PlayerSummonController.IsMouseOverControllableSummon())
        {
            return true;
        }
        if (PlayerSummonController.HadSelectionThisFrame())
        {
            return true;
        }
        if (PlayerSummonController.MouseOverUIComponent())
        {
            return true;
        }
        return false;
    }

    //so, yes this is exposing a detail of operation, but it kind of makes sense. 
    public void SetAttackFrame(int frame)
    {
        attackFrame = frame; 
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
            if (deselectAfter)
            {
                ItemSelection.DeselectItem();
            }
            //this is kind of interesting - this runs after the current state has changed. 
        }
    }

    public void ExitState()
    {

    }
}
