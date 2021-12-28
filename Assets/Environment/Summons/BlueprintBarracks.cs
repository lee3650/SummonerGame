using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintBarracks : BlueprintSatisfier
{
    [SerializeField] Vector2Int SpawnOffset; //we are going to want to draw a preview for that.  
    private static List<Vector2Int> SpawnPoints = new List<Vector2Int>();

    private Vector2Int SpawnPos;

    public static bool IsPointSpawnPoint(Vector2Int point)
    {
        return SpawnPoints.Contains(point);
    }

    public override void Init()
    {
        SpawnPos = VectorRounder.RoundVectorToInt(transform.position) + GetSpawnOffset();
        SpawnPoints.Add(SpawnPos);
        print("Added " + SpawnPos + " to spawn points!");
        base.Init();
    }

    protected override GameObject SummonEntity(GameObject entity, Vector2 endPoint, float rotation)
    {
        GameObject summon = SummonWeapon.SpawnSummon(entity, (Vector2)transform.position + SpawnOffset, MySummon.GetSummoner(), Quaternion.Euler(Vector2.zero));
        summon.GetComponent<IControllableSummon>().HandleCommand(new HoldPointCommand(endPoint));
        return summon; 
    }

    public override bool CanBeSold()
    {
        Vector2Int pos = VectorRounder.RoundVectorToInt(transform.position);
        return AdjacentConnections.DoAdjacentTilesConnectToMiner(new List<Vector2Int> { pos, pos + GetSpawnOffset() });
    }

    protected override void OnDeath()
    {
        RemovePointFromSpawnPoints();
        base.OnDeath();
    }

    public Vector2Int GetSpawnOffset()
    {
        return SpawnOffset;
    }

    private void RemovePointFromSpawnPoints()
    {
        SpawnPoints.Remove(SpawnPos);
        print("removed " + SpawnPos + " from spawn points!");
        print("SpawnPoints count:  " + SpawnPoints.Count);
    }

    private void OnDestroy()
    {
        RemovePointFromSpawnPoints();
    }
}
