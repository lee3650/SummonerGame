using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackManager : MonoBehaviour
{
    [SerializeField] AIAttackState AttackState;
    [SerializeField] AIStateMachine AIStateMachine;

    public virtual void TryAttack(ILivingEntity CurrentTarget)
    {
        if (Vector2.Distance(CurrentTarget.GetPosition(), transform.position) < AttackState.GetAttackRange())
        {
            AIStateMachine.TransitionToState(AttackState);
        }
    }
}
