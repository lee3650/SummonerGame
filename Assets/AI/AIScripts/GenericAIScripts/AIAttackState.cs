using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackState : MonoBehaviour, IState
{
    [SerializeField] float AttackLength;
    [SerializeField] float AttackRange;
    [SerializeField] bool RotateWhileAttacking = true;
    [SerializeField] bool MoveWhileAttacking = false;

    [SerializeField] AIStateMachine AIStateMachine;
    [SerializeField] AIPursuitState AIPursuitState;

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
            AIStateMachine.TransitionToState(AIPursuitState);
        }
    }

    public void ExitState()
    {

    }

    public virtual void StartAttack()
    {
        //so, this would be, either spawn projectile, or 
        //start animation for melee. 

        //probably want to... not do anything here and subclass this. 

    }

    public float GetAttackRange()
    {
        return AttackRange;
    }
}
