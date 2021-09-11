using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceHomeState : MonoBehaviour, IState
{
    [SerializeField] HomeTileSummon HomeSummonPrefab;
    [SerializeField] PlayerAttackState PlayerAttackState; 
    [SerializeField] Component StateToExitTo;
    [SerializeField] PlayerInput PlayerInput;
    [SerializeField] PlayerStateController PlayerStateController;
    [SerializeField] HealthManager health;
    [SerializeField] MovementController MovementController;
    [SerializeField] RotationController RotationController;
    [SerializeField] GameObject NextWaveButton; 

    private HomeTileSummon actualHomeSummon;

    bool selected = false; 

    public void EnterState()
    {
        actualHomeSummon = Instantiate(HomeSummonPrefab);
        actualHomeSummon.Initialize(health, transform);
    }

    public void UpdateState()
    {
        Vector2 unitMoveDir = PlayerInput.GetUnitInputDirection();
        MovementController.MoveInDirection(unitMoveDir);

        RotationController.FacePoint(PlayerInput.GetWorldMousePosition());

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

    bool AttackConditionsMet()
    {
        return PlayerAttackState.IsPositionSpawnable() && PlayerAttackState.IsWeaponUseable(actualHomeSummon) && PlayerAttackState.InventoryIsValid(actualHomeSummon);
    }

    public void ExitState()
    {
        NextWaveButton.SetActive(true); //this is kinda lame but whatever 
        actualHomeSummon.GetComponent<SummonWeapon>().OnDeselection();
        Destroy(actualHomeSummon.gameObject);
    }
}
