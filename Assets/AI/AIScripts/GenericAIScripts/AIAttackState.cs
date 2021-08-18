using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackState : MonoBehaviour, IState, IRanged
{
    [SerializeField] float AttackLength;
    [SerializeField] float AttackRange;
    [SerializeField] bool RotateWhileAttacking = true;
    [SerializeField] bool MoveWhileAttacking = false;

    [SerializeField] AIStateMachine AIStateMachine;
    [SerializeField] Component StateToExitTo;

    [SerializeField] TargetManager TargetManager;

    protected AIEntity AIEntity;

    float AttackTimer = 0f;

    private void Awake()
    {
        AIEntity = GetComponent<AIEntity>();
    }

    public void EnterState()
    {
        StartAttack();
        AttackTimer = 0f;
        TargetManager.LookAtTarget();
    }

    public void UpdateState()
    {
        AttackTimer += Time.deltaTime;

        if (RotateWhileAttacking)
        {
            TargetManager.LookAtTarget();
        }

        if (MoveWhileAttacking)
        {
            TargetManager.MoveAtTarget();
        }

        if (AttackTimer > AttackLength)
        {
            AIStateMachine.TransitionToState(StateToExitTo as IState);
        }
    }

    public void ExitState()
    {
        EndAttack();
    }

    public virtual void EndAttack()
    {

    }
    public virtual void StartAttack()
    {
        //so, this would be, either spawn projectile, or 
        //start animation for melee. 

        //probably want to... not do anything here and subclass this. 

    }

    public float GetRange()
    {
        return AttackRange;
    }
}
