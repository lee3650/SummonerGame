using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackManager : MonoBehaviour
{
    [SerializeField] AIAttackState AttackState;
    [SerializeField] AIStateMachine AIStateMachine;

    public bool IsTargetInRange(ITargetable CurrentTarget)
    {
        if (Vector2.Distance(CurrentTarget.GetPosition(), transform.position) <= AttackState.GetAttackRange())
        {
            return true;
        }
        return false; 
    }

    public virtual void TryAttack(ITargetable CurrentTarget)
    {
        if (CanAttack(CurrentTarget))
        {
            AIStateMachine.TransitionToState(AttackState);
        }
    }

    protected virtual bool CanAttack(ITargetable CurrentTarget)
    {
        return IsTargetInRange(CurrentTarget);
    }
}
