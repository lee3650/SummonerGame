﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintBarracks : BlueprintSatisfier
{
    [SerializeField] Vector2Int SpawnOffset; //we are going to want to draw a preview for that.  

    protected override GameObject SummonEntity(GameObject entity, Vector2 endPoint)
    {
        GameObject summon = SummonWeapon.SpawnSummon(entity, (Vector2)transform.position + SpawnOffset, MySummon.GetSummoner(), Quaternion.Euler(Vector2.zero));
        summon.GetComponent<IControllableSummon>().HandleCommand(new HoldPointCommand(endPoint));
        return summon; 
    }

    public Vector2Int GetSpawnOffset()
    {
        return SpawnOffset;
    }
}
