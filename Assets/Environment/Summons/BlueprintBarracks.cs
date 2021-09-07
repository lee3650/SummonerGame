using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintBarracks : BlueprintSatisfier
{
    [SerializeField] Vector2Int SpawnOffset; //we are going to want to draw a preview for that.  

    protected override GameObject SummonEntity(GameObject entity, Vector2 endPoint)
    {
        GameObject summon = SummonWeapon.SpawnSummon(entity, (Vector2)transform.position + SpawnOffset, MySummon.GetSummoner(), Quaternion.Euler(Vector2.zero));
        summon.GetComponent<IControllableSummon>().HandleCommand(new HoldPointCommand(endPoint));
        TargetableEntitiesManager.AddSecondaryTarget(VectorRounder.RoundVectorToInt(endPoint), summon.GetComponent<ITargetable>()); //eh, that's not that bad. We just know that the summon has a targetable component. 
        //okay... this should be fine. I'll admit it's a little sketchy but it's better than looping through like 100 guys for every tile in the path, you know? 
        return summon; 
    }

    public Vector2Int GetSpawnOffset()
    {
        return SpawnOffset;
    }
}
