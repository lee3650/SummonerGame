using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDrawer : MonoBehaviour
{
    [SerializeField] List<TileToObject> Tiles;

    GameObject[,] DrawnMap = null;

    Dictionary<TileType, GameObject> TileToPrefab = new Dictionary<TileType, GameObject>(); 

    private void Awake()
    {
        foreach (TileToObject tileToObject in Tiles)
        {
            TileToPrefab[tileToObject.TileType] = tileToObject.GameObject;
        }
    }

    public void DestroyOldMap()
    {
        if (DrawnMap == null)
        {
            return;
        }

        for (int x = 0; x < MapManager.xSize; x++)
        {
            for (int y = 0; y < MapManager.ySize; y++)
            {
                Destroy(DrawnMap[x, y].gameObject);
            }
        }

        DrawnMap = null; 
    }

    public void InstantiateMap(MapNode[,] map)
    {
        DrawnMap = new GameObject[MapManager.xSize, MapManager.ySize];

        for (int x = 0; x < MapManager.xSize; x++)
        {
            for (int y = 0; y < MapManager.ySize; y++)
            {
                DrawnMap[x, y] = Instantiate(TileToPrefab[map[x,y].TileType], new Vector3(x, y), Quaternion.Euler(Vector3.zero));
            }
        }
    }
}
