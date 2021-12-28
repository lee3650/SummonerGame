using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitWriteTile : MonoBehaviour, IInitialize
{
    [SerializeField] PlayerWall PlayerWall;
    [SerializeField] Vector2Int offset;
    [SerializeField] HealthManager HealthManager;

    private MapNode prevNode;
    Vector2Int pos;

    public void Init()
    {
        pos = VectorRounder.RoundVectorToInt(transform.position) + offset;
        prevNode = MapManager.GetMap()[pos.x, pos.y];
        MapManager.WritePoint(pos.x, pos.y, new MapNode(true, PlayerWall.GetTileType()));
        HealthManager.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        MapManager.WritePoint(pos.x, pos.y, prevNode);
    }
}
