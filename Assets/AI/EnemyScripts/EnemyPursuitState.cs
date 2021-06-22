using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPursuitState : MonoBehaviour, IState
{
    [SerializeField] TargetManager TargetManager;

    ILivingEntity CurrentTarget;

    public void EnterState()
    {
        CurrentTarget = TargetManager.Target;
        if (CurrentTarget == null)
        {
            throw new System.Exception("Entered pursuit state with null target!");
        }
    }

    public void UpdateState()
    {
        while (CurrentTarget.IsAlive())
        {

        }
    }

    public void ExitState()
    {

    }
}
