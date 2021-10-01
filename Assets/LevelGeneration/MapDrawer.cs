using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDrawer : MonoBehaviour
{
    [SerializeField] List<TileToSprites> Tiles;
    [SerializeField] GameObject EmptyPrefab; 

    GameObject[,] DrawnMap = null;

    Dictionary<TileType, GameObject[]> TileToPrefab = new Dictionary<TileType, GameObject[]>();

    List<GameObject> ExtraWalls = new List<GameObject>();

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
    
    public void ConditionallyDestroyTiles()
    {
        for (int x = 0; x < DrawnMap.GetLength(0); x++)
        {
            for (int y = 0; y < DrawnMap.GetLength(1); y++)
            {
                if (DrawnMap[x, y] != null)
                {
                    ConditionalDelete cd;
                    if (DrawnMap[x,y].TryGetComponent<ConditionalDelete>(out cd))
                    {
                        if (cd.TryDestroy())
                        {
                            DrawnMap[x, y] = null;
                        }
                    }
                }
            }
        }

        for (int i = ExtraWalls.Count - 1; i >= 0; i--)
        {
            ConditionalDelete cd;
            if (ExtraWalls[i] != null && ExtraWalls[i].TryGetComponent<ConditionalDelete>(out cd))
            {
                if (cd.TryDestroy())
                {
                    ExtraWalls.RemoveAt(i);
                }
            }
        }
    }

    public void DrawEnclosingWalls(int xSize, int ySize)
    {
        for (int x = -1; x <= xSize; x++)
        {
            ExtraWalls.Add(GetInstantiatedTile(x, -1, new MapNode(false, TileType.Wall)));
            ExtraWalls.Add(GetInstantiatedTile(x, ySize, new MapNode(false, TileType.Wall)));
        }
        for (int y = 0; y < ySize; y++)
        {
            ExtraWalls.Add(GetInstantiatedTile(-1, y, new MapNode(false, TileType.Wall)));
            ExtraWalls.Add(GetInstantiatedTile(xSize, y, new MapNode(false, TileType.Wall)));
        }
    }
    
    public void DestroyTiles(List<Vector2> tiles)
    {
        foreach (Vector2 t in tiles)
        {
            if (DrawnMap[(int)t.x, (int)t.y] != null)
            {
                Destroy(DrawnMap[(int)t.x, (int)t.y].gameObject);
            }
        }
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
                DrawnMap[x, y] = GetInstantiatedTile(x, y, map[x, y]); //okay, so this just uses x and y to place, and then map[x,y] to find the tile type. 
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

    public void InstantiateNonCenteredMap(MapNode[,] map, Vector2Int worldOrigin)
    {
        int xSize = map.GetLength(0);
        int ySize = map.GetLength(1);

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                Vector2Int worldPoint = new Vector2Int(x, y) + worldOrigin;
                GameObject g = GetInstantiatedTile(worldPoint.x, worldPoint.y, map[x, y]);

                SetSpriteWangTiles w;
                if (g.TryGetComponent<SetSpriteWangTiles>(out w))
                {
                    w.InjectMap(map, worldOrigin);
                }
            }
        }
    }
}