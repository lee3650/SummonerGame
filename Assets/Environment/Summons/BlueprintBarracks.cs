using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintBarracks : BlueprintSatisfier
{
    protected override GameObject SummonEntity(GameObject entity, Vector2 endPoint)
    {
        GameObject summon = SummonWeapon.SpawnSummon(entity, (Vector2)transform.position, MySummon.GetSummoner(), Quaternion.Euler(Vector2.zero));
        summon.GetComponent<IControllableSummon>().HandleCommand(new HoldPointCommand(endPoint));

        return summon; 
    }
}
