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
    [SerializeField] Component StateToExitTo;

    [SerializeField] protected TargetManager TargetManager;
    [SerializeField] protected DirectionalAnimator Animator;

    private float InternalAttackLength;

    protected AIEntity AIEntity;

    float AttackTimer = 0f;

    private void Awake()
    {
        AIEntity = GetComponent<AIEntity>();
        InternalAttackLength = AttackLength;
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

        if (AttackTimer > InternalAttackLength)
        {
            AIStateMachine.TransitionToState(StateToExitTo as IState);
        }
    }

    public void ExitState()
    {
        EndAttack();
    }

    public void MultiplyAttackLength(float multiplier)
    {
        InternalAttackLength = multiplier * AttackLength;
    }

    public void ResetAttackLength()
    {
        InternalAttackLength = AttackLength;
    }

    public virtual void EndAttack()
    {
        if (Animator != null)
        {
            Animator.IdleFacePoint(TargetManager.Target.GetPosition());
        }
    }

    public virtual void StartAttack()
    {
        Animator.PlayAttack(TargetManager.Target.GetPosition());
    }

    public float GetRange()
    {
        return AttackRange;
    }
}
