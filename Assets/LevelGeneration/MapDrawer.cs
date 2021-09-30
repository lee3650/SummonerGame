using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDrawer : MonoBehaviour
{
    [SerializeField] List<TileToSprites> Tiles;
    [SerializeField] GameObject EmptyPrefab; 

    GameObject[,] DrawnMap = null;

    Dictionary<TileType, GameObject[]> TileToPrefab = new Dictionary<TileType, GameObject[]>(); 

    private void Awake()
    {
        foreach (TileToSprites tileToSprites in Tiles)
        {
            if (tileToSprites.Sprites.Length != 0)
            {
                GameObject[] images = new GameObject[tileToSprites.Sprites.Length];

                for (int i = 0; i < tileToSprites.Sprites.Length; i++)
                {
                    Sprite s = tileToSprites.Sprites[i];
                    GameObject pref = tileToSprites.UseDefaultObject ? EmptyPrefab : tileToSprites.OverridenPrefab;
                    GameObject prefab = Instantiate(pref, new Vector3(-10, -10, 0f), Quaternion.Euler(Vector3.zero)); //I'm not sure if this is necessary. 
                    prefab.GetComponent<SpriteRenderer>().sprite = s;
                    images[i] = prefab;
                }

                TileToPrefab[tileToSprites.TileType] = images;
            }
        }
    }
    
    public void DrawEnclosingWalls(int xSize, int ySize)
    {
        for (int x = -1; x <= xSize; x++)
        {
            GetInstantiatedTile(x, -1, new MapNode(false, TileType.Wall));
            GetInstantiatedTile(x, ySize, new MapNode(false, TileType.Wall));
        }
        for (int y = 0; y < ySize; y++)
        {
            GetInstantiatedTile(-1, y, new MapNode(false, TileType.Wall));
            GetInstantiatedTile(xSize, y, new MapNode(false, TileType.Wall));
        }
    }

    public void DestroyTiles(List<Vector2> tiles)
    {
        foreach (Vector2 t in tiles)
        {
            Destroy(DrawnMap[(int)t.x, (int)t.y].gameObject);
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

    public void RewritePoint(int x, int y, MapNode mapNode)
    {
        if (x >= 0 && x < MapManager.xSize && y >= 0 && y < MapManager.ySize)
        {
            DrawnMap[x, y] = GetInstantiatedTile(x, y, mapNode);
        }
    }

    public void InstantiatePartOfMap(MapNode[,] map, Vector2 start, Vector2 end)
    {
        for (int x = (int)start.x; x < (int)end.x; x++)
        {
            for (int y = (int)start.y; y < (int)end.y; y++)
            {
                DrawnMap[x, y] = GetInstantiatedTile(x, y, map[x, y]);
            }
        }
    }

    GameObject GetInstantiatedTile(int x, int y, MapNode tile)
    {
        return Instantiate(TileToPrefab[tile.TileType][Random.Range(0, TileToPrefab[tile.TileType].Length)], new Vector2(x, y), Quaternion.Euler(Vector3.zero));
    }

    public void InstantiateMap(MapNode[,] map)
    {
        DrawnMap = new GameObject[MapManager.xSize, MapManager.ySize];

        InstantiatePartOfMap(map, new Vector2(0, 0), new Vector2(MapManager.xSize, MapManager.ySize));        
    }
}
