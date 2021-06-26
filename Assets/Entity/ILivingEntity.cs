using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILivingEntity : IEntity
{
    Factions GetFaction();
    int GetPrecedence();

    bool IsAlive();

    Vector2 GetPosition();

}