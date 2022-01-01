using System.Linq;
using UnityEngine;

public abstract class Charm : MonoBehaviour
{
    [SerializeField] CharmType CharmType;
    [SerializeField] SummonType[] ApplyToTypes;

    //does nothing by default 
    public virtual Event GetCharmModifiedEvent(Event e)
    {
        return e; 
    }

    public abstract Event GetAttackModifier(IEntity sender);

    public bool ApplyToType(SummonType type)
    {
        return ApplyToTypes.Contains(type) || ApplyToTypes.Contains(SummonType.Any);
    }

    public bool HasAttackModifier()
    {
        return CharmType == CharmType.AttackModifier; //so eNcApsulAtEd
    }
}
