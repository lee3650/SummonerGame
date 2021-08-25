using System.Linq;
using UnityEngine;

//doesn't this need to be an item? 
public abstract class Charm : Item
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
