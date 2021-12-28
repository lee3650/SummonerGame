using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldPointState : MonoBehaviour, IControllableState
{
    [SerializeField] MovementController MovementController;
    [SerializeField] RotationController RotationController;
    [SerializeField] TargetManager TargetManager;
    [SerializeField] StateController StateController;
    [SerializeField] AIAttackManager AIAttackManager;
    [SerializeField] DirectionalAnimator DirectionalAnimator;

    Rigidbody2D rb; 

    PointToHoldManager PointToHoldManager;

    [SerializeField] private bool waitingForPath = false;

    float noPathTimer = 0f;

    void Awake()
    {
        PointToHoldManager = GetComponent<PointToHoldManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void EnterState()
    {
        if (PointToHoldManager == null)
        {
            PointToHoldManager = GetComponent<PointToHoldManager>();
        }
        print("Entered hold point state! Point to hold is " + PointToHoldManager.PointToHold);
        
        if (PointToHoldManager.Initialized)
        {
            MovementController.SetPathfindGoal(PointToHoldManager.PointToHold);
        }
        
        if (!MovementController.FoundPath() && PointToHoldManager.Initialized)
        {
            waitingForPath = true;
            noPathTimer = 1f; 
            CritGraphicPool.ShowNoPath(PointToHoldManager.PointToHold + new Vector2(0, 1));
        } else
        {
            waitingForPath = false; 
        }
    }

    public void UpdateState()
    {
        MovementController.MoveTowardPointWithRotation(PointToHoldManager.PointToHold);

        if (rb.velocity.sqrMagnitude > 0.5f)
        {
            DirectionalAnimator.PlayWalk(rb.velocity);
        } else
        {
            DirectionalAnimator.IdleFacePoint((Vector2)transform.position + new Vector2(0, -1));
        }

        if (TargetManager.HasLivingTarget() && Vector2.Distance(transform.position, PointToHoldManager.PointToHold) < 2.5f)
        {
            AIAttackManager.TryAttack(TargetManager.Target);
        }

        if (waitingForPath)
        {
            noPathTimer -= Time.deltaTime;
            if (noPathTimer <= 0)
            {
                CritGraphicPool.ShowNoPath(PointToHoldManager.PointToHold + new Vector2(0, 1));
                noPathTimer = 1f; 
            }
            
            if (PathsChanged.RecalculatePathfinding())
            {
                MovementController.SetPathfindGoal(PointToHoldManager.PointToHold);
                if (MovementController.FoundPath())
                {
                    waitingForPath = false;
                }
            }
        }
    }

    public void ExitState()
    {

    }

    public void HandleCommand(PlayerCommand command)
    {
        switch (command)
        {
            case HoldPointCommand hp:
                //MovementController.SetPathfindGoal(hp.PointToHold);
                EnterState();
                break;
            case ToggleGuardModeCommand tg:
                break;
            case RestCommand rc:
                GetComponent<ControllableSummon>().TransitionToRestState();
                break;
            case DeactivateCommand dc:
                GetComponent<ControllableSummon>().TransitionToDeactivatedState();
                break;
        }
    }
}
