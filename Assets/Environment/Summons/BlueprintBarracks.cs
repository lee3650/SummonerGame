using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//really need to rename WallGenerator 
public class BlueprintBarracks : WallGenerator
{
    protected override GameObject SummonEntity(GameObject entity, Vector2 endPoint)
    {
        GameObject summon = SummonWeapon.SpawnSummon(entity, (Vector2)transform.position + new Vector2(1, 0), MySummon.GetSummoner(), Quaternion.Euler(Vector2.zero));
        summon.GetComponent<IControllableSummon>().HandleCommand(new HoldPointCommand(endPoint));

        return summon; 
    }
}
