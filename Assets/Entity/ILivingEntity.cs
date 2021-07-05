using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILivingEntity : IEntity, ITargetable, IWielder
{
    Factions GetFaction();
}