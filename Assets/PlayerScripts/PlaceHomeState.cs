using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHomeState : MonoBehaviour, IState
{
    [SerializeField] PlayerMoveState MoveState;
    [SerializeField] HomeTileSummon HomeSummonPrefab;
    [SerializeField] PlayerAttackState PlayerAttackState; 
    [SerializeField] Component StateToExitTo;
    [SerializeField] PlayerInput PlayerInput;
    [SerializeField] PlayerStateController PlayerStateController;
    [SerializeField] HealthManager health;
    [SerializeField] MovementController MovementController;
    [SerializeField] RotationController RotationController;
    [SerializeField] GameObject NextWaveButton;
    [SerializeField] Summoner Summoner;
    [SerializeField] GameObject Hotbar;
    [SerializeField] BlueprintFees BlueprintFees;

    private HomeTileSummon actualHomeSummon;

    bool selected = false; 

    public void EnterState()
    {
        actualHomeSummon = Instantiate(HomeSummonPrefab);
        actualHomeSummon.Initialize(health, transform);
    }

    public void UpdateState()
    {
        //Vector2 unitMoveDir = PlayerInput.GetUnitInputDirection();
        //MovementController.MoveInDirection(unitMoveDir);

        //RotationController.FacePoint(PlayerInput.GetWorldMousePosition());

        MoveState.MoveWithMouse();

        if (MapManager.IsMapInitialized())
        {
            if (!selected)
            {
                actualHomeSummon.OnSelection();
                selected = true; 
            }
            
            actualHomeSummon.UpdatePreview(AttackConditionsMet(), PlayerInput.GetWorldMousePosition());
        }

        if (Input.GetMouseButtonDown(0) && MapManager.IsMapInitialized())
        {
            if (AttackConditionsMet())
            {
                actualHomeSummon.UseWeapon(VectorRounder.RoundVector(PlayerInput.GetWorldMousePosition()));
                PlayerStateController.TransitionToState(StateToExitTo as IState);
                PlayerAttackState.SetAttackFrame(Time.frameCount);
            }
        }
    }

    public bool IsSelected
    {
        get
        {
            return selected;
        }
    }

    public HomeTileSummon GetHomeTileSummon()
    {
        if (selected)
        {
            return actualHomeSummon;
        }
        return null;
    }

    bool AttackConditionsMet()
    {
        return PlayerAttackState.IsPositionSpawnable(PlayerInput.GetWorldMousePosition()) && PlayerAttackState.IsWeaponUseable(actualHomeSummon, PlayerInput.GetWorldMousePosition()) && PlayerAttackState.InventoryIsValid(actualHomeSummon);
    }

    public void ExitState()
    {
        selected = false;
        BlueprintFees.InitializePrices(); //technically could be static.
        NextWaveButton.SetActive(true); //this is kinda lame but whatever 
        Hotbar.SetActive(true);
        actualHomeSummon.GetComponent<SummonWeapon>().OnDeselection();
        Destroy(actualHomeSummon.gameObject);
        Summoner.OnFinancialsChanged();
        MinerSummon.ScaleCost(Summoner);
    }
}
