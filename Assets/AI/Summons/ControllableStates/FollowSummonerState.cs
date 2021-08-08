using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSummonerState : MonoBehaviour, IControllableState
{
    Summon Summon;
    [SerializeField] MovementController MovementController;
    [SerializeField] RotationController RotationController;
    [SerializeField] TargetManager TargetManager;
    [SerializeField] StateController StateController;
    [SerializeField] AIPursuitState AIPursuitState;

    private void Awake()
    {
        Summon = GetComponent<Summon>();
    }

    public void EnterState()
    {

    }

    public void UpdateState()
    {
        if (Vector2.Distance(Summon.GetSummonerPosition(), transform.position) > 6f)
        {
            MoveTowardsSummoner();
            RotationController.FaceForward();
        } else
        {
            LookAtSummoner();
        }

        if (TargetManager.HasLivingTarget())
        {
            StateController.TransitionToState(AIPursuitState);
        }
    }

    public void HandleCommand(PlayerCommand command)
    {

    }

    void LookAtSummoner()
    {
        RotationController.FacePoint(Summon.GetSummonerPosition());
    }

    void MoveTowardsSummoner()
    {
        Vector2 dir = Summon.GetSummonerPosition() - (Vector2)transform.position;
        MovementController.MoveInDirection(dir);
    }

    public void ExitState()
    {

    }
}
