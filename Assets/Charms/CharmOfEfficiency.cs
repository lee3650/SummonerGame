using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmOfEfficiency : Charm
{
    public float EfficiencyModifier; 
    public override Event GetAttackModifier(IEntity sender)
    {
        return new Event(EventType.Physical, 0f, sender);
    }
}
