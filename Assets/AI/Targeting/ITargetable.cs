using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable : IEntity
{
    int GetPrecedence();

    bool IsAlive();

    Vector2 GetPosition();

    bool CanBeTargeted();

    Transform GetTransform();

    bool IsDamaged();

    bool RequireLineOfSight();

    bool CanBeTargetedBy(Factions faction);
}
