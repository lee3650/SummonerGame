using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeAdder : MonoBehaviour
{
    [SerializeField] GameObject upBridge;
    [SerializeField] GameObject horBridge;

    private static List<Vector2Int> BridgeTiles = new List<Vector2Int>();
    private static List<bool> BridgeDirections = new List<bool>();

    static GameObject UpBridge;
    static GameObject HorBridge;

    private void Awake()
    {
        HorBridge = horBridge;
        UpBridge = upBridge;
    }

    public static void ResetBridges()
    {
        BridgeTiles = new List<Vector2Int>();
        BridgeDirections = new List<bool>();
    }

    public static void AddBridge(Vector2Int bridge, bool upbridge)
    {
        BridgeTiles.Add(bridge);
        BridgeDirections.Add(upbridge);
    }

    public static void WriteBridges()
    {
        List<BridgeTile> tiles = new List<BridgeTile>();

        //we have to write all the tiles first or the colliders won't be disabled properly 
        for (int i = 0; i < BridgeTiles.Count; i++)
        {
            Vector2Int b = BridgeTiles[i];
            if (MapManager.GetTileType(b) == TileType.DoNotDraw)
            {
                MapManager.WritePoint(b.x, b.y, new MapNode(true, TileType.Bridge));
            }
        }

        for (int i = 0; i < BridgeTiles.Count; i++)
        {
            Vector2Int b = BridgeTiles[i];
            if (MapManager.GetTileType(b) == TileType.Bridge) 
            {
                GameObject prefab;

                if (BridgeDirections[i])
                {
                    prefab = UpBridge;
                }
                else
                {
                    prefab = HorBridge;
                }

                BridgeTile t = Instantiate(prefab, new Vector3(b.x, b.y, 0), Quaternion.Euler(new Vector3(0, 0, 0))).GetComponent<BridgeTile>();
                tiles.Add(t);
            }
        }

        foreach (BridgeTile t in tiles)
        {
            t.SetGraphic();
        }
    }
}
