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

    private HomeTileSummon actualHomeSummon;

    public void EnterState()
    {
        actualHomeSummon = Instantiate(HomeSummonPrefab);
        actualHomeSummon.Initialize(health, transform);
        actualHomeSummon.OnSelection();
    }

    public void UpdateState()
    {
        if (MapManager.IsMapInitialized())
        {
            actualHomeSummon.UpdatePreview(AttackConditionsMet(), PlayerInput.GetWorldMousePosition());
        }

        if (Input.GetMouseButtonDown(0))
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
        actualHomeSummon.GetComponent<SummonWeapon>().OnDeselection();
        Destroy(actualHomeSummon.gameObject);
    }
}
