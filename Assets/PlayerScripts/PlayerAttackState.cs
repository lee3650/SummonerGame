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
            DoAttack();
            SummonWeapon w = ItemSelection.SelectedItem as SummonWeapon;
            if (w != null)
            {
                w.UpdatePreview(false, Vector2.zero);
            }
        } else
        {
            StateController.TransitionToState(PlayerMoveState);
        }
    }
    
    private void DoAttack()
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

        if (!IsPositionSpawnable(PlayerInput.GetWorldMousePosition()))
        {
            return false;
        }

        if (PlayerIsSelecting())
        {
            return false;
        }

        if (!IsWeaponUseable(weapon, PlayerInput.GetWorldMousePosition()))
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

    public bool IsWeaponUseable(Weapon weapon, Vector2 pos)
    {
        if (weapon == null)
        {
            return false; 
        }
        return weapon.CanUseWeapon(pos);
    }

    bool SufficientEnergy(Weapon weapon)
    {
        return ManaManager.GetCurrent() >= weapon.GetManaDrain(); //HealthManager.GetCurrent() + 
    }

    public bool IsPositionSpawnable(Vector2 mousePos)
    {
        return MapManager.IsPointTraversable(mousePos, true) && !WaveSpawner.IsPointInSpawnRegion(VectorRounder.RoundVector(mousePos));
    }

    bool PlayerIsSelecting()
    {
        if (PlayerSummonController.IsMouseOverControllableSummon())
        {
            return true;
        }
        /*
        if (PlayerSummonController.HadSelectionThisFrame())
        {
            return true;
        }
         */
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

        Weapon weapon = (ItemSelection.SelectedItem as Weapon);

        if (timer >= weapon.GetAttackLength())
        {
            if (Input.GetMouseButton(0) && weapon.AllowRepeatAttack())
            {
                if (AttackConditionsMet())
                {
                    timer = 0f;
                    DoAttack();
                }
            }
            else
            {
                StateController.TransitionToState(PlayerMoveState);
                if (deselectAfter)
                {
                    ItemSelection.DeselectItem();
                }
            }
            //this is kind of interesting - this runs after the current state has changed.
            //I'm not sure why this isn't in exit state
        }
    }

    public void ExitState()
    {

    }
}
