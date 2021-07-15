using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Charm : MonoBehaviour
{
    [SerializeField] bool AttackModifier;
    [SerializeField] SummonType[] ApplyToTypes;

    //does nothing by default 
    public virtual Event GetCharmModifiedEvent(Event e)
    {
        return e; 
    }
    public abstract Event GetAttackModifier();

    public bool ApplyToType(SummonType type)
    {
        return ApplyToTypes.Contains(type) || ApplyToTypes.Contains(SummonType.Any);
    }

    public bool HasAttackModifier()
    {
        return AttackModifier;
    }
}
