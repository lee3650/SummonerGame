using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEnemyCoatingManager : CoatingManager
{
    public override void SetCoating(Coating newCoating)
    {
        if (newCoating is WaterCoating)
        {
            return; 
        }

        base.SetCoating(newCoating);
    }
}
