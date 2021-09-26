using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackManager : MonoBehaviour, IRanged
{
    [SerializeField] AIAttackState AttackState;
    [SerializeField] AIStateMachine AIStateMachine;
    [SerializeField] bool ApplyDamage = false; 

    public bool IsTargetInRange(ITargetable CurrentTarget)
    {
        if (Vector2.Distance(CurrentTarget.GetPosition(), transform.position) <= AttackState.GetRange())
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

    public virtual bool IsCrossShaped()
    {
        return false;
    }

    public virtual float GetCrossDelta()
    {
        return 0f;
    }

    public float GetRange()
    {
        return AttackState.GetRange();
    }

    public virtual bool CanAttack(ITargetable CurrentTarget)
    {
        return IsTargetInRange(CurrentTarget);
    }

}
