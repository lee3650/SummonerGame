using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmOfDamageModification : Charm
{
    [SerializeField] EventType additionalEventType;
    [SerializeField] float eventMagnitude;

    public override Event GetAttackModifier(IEntity sender)
    {
        return new Event(additionalEventType, eventMagnitude, sender);
    }
}
