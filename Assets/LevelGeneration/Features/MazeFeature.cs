using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeFeature : MapFeature
{
    Dictionary<char, Vector2Int> DirToVector = new Dictionary<char, Vector2Int>();
    Dictionary<char, char> Opposites = new Dictionary<char, char>();

    private const int cellSize = 5;

    public override void AddFeature(int xSize, int ySize, MapNode[,] map)
    {
        //so, we're probably going to want to make a new representation of the map. 
        //We can just do the classic char map and then draw the walls based on that. 


        EntranceAndExitDir[,] maze = new EntranceAndExitDir[xSize/cellSize, ySize/cellSize];

        maze[0, 0] = new EntranceAndExitDir(new Vector2Int());

        DirToVector['N'] = new Vector2Int(0, 1);
        DirToVector['S'] = new Vector2Int(0, -1);
        DirToVector['E'] = new Vector2Int(1, 0);
        DirToVector['W'] = new Vector2Int(-1, 0);

        Opposites['N'] = 'S';
        Opposites['S'] = 'N';
        Opposites['E'] = 'W';
        Opposites['W'] = 'E';

        CarvePassages(0, 0, maze);

        //eh why even test this. Let's just go for it. 

        DrawMazeOverMap(maze, map);
    }

    private void DrawMazeOverMap(EntranceAndExitDir[,] maze, MapNode[,] map)
    {
        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                //so, we draw walls in every direction except the ones that correspond to 
                //entrance and exit direction... 

                //okay how about this lol - conceptually, it's like pushing a box through the scene. 
                //a box that is 3x3 and has a bottom left corner at (relative) 1,1. 
                //okay. We can use that. 

                DrawMazeWalls(x * cellSize, y * cellSize, maze[x, y], map);    
            }
        }
    }

    private void DrawMazeWalls(int sx, int sy, EntranceAndExitDir dir, MapNode[,] map)
    {
        for (int dx = 0; dx < cellSize; dx++)
        {
            if (ShouldPlaceWallAtPoint(dx, 0, dir))
            {
                map[sx + dx, sy] = new MapNode(false, TileType.Valley);
            }

            if (ShouldPlaceWallAtPoint(dx, cellSize - 1, dir))
            {
                map[sx + dx, sy + cellSize - 1] = new MapNode(false, TileType.Valley);
            }
        }

        for (int dy = 0; dy < cellSize; dy++)
        {
            if (ShouldPlaceWallAtPoint(0, dy, dir))
            {
                map[sx, sy + dy] = new MapNode(false, TileType.Valley);
            }

            if (ShouldPlaceWallAtPoint(cellSize - 1, dy, dir))
            {
                map[sx + cellSize - 1, sy + dy] = new MapNode(false, TileType.Valley);
            }
        }
    }

    private bool ShouldPlaceWallAtPoint(int dx, int dy, EntranceAndExitDir dirs)
    {

        char[,] tile = new char[cellSize, cellSize];
        for (int x = 1; x < cellSize - 1; x++)
        {
            for (int y = 1; y < cellSize - 1; y++)
            {
                tile[x, y] = 'c';

                if (x + dirs.EntranceDir.x < cellSize && y + dirs.EntranceDir.y < cellSize && x + dirs.EntranceDir.x >= 0 && y + dirs.EntranceDir.y >= 0)
                {
                    tile[x + dirs.EntranceDir.x , y + dirs.EntranceDir.y] = 'c';
                }

                if (x + dirs.ExitDir.x < cellSize && y + dirs.ExitDir.y < cellSize && x + dirs.ExitDir.x >= 0 && y + dirs.ExitDir.y >= 0)
                {
                    tile[x + dirs.ExitDir.x, y + dirs.ExitDir.y] = 'c';
                }
            }
        }

        return tile[dx, dy] != 'c';
    }

    private void CarvePassages(int x, int y, EntranceAndExitDir[,] map)
    {
        char[] dirs = new char[] { 'N', 'S', 'E', 'W' };
        
        dirs = (char[])ListRandomSort<char>.SortListRandomly(dirs);

        foreach (char dir in dirs)
        {
            Vector2Int next = new Vector2Int(x, y) + DirToVector[dir];

            if (next.x >= 0 && next.y >= 0 && next.x < map.GetLength(0) && next.y < map.GetLength(1) && map[next.x, next.y] == null)
            {
                map[x, y].ExitDir = DirToVector[dir];
                map[next.x, next.y] = new EntranceAndExitDir(DirToVector[Opposites[dir]]);

                CarvePassages(next.x, next.y, map);
            }
        }
    }
}

class EntranceAndExitDir
{
    public Vector2Int EntranceDir;
    public Vector2Int ExitDir;

    public EntranceAndExitDir(Vector2Int entranceDir)
    {
        EntranceDir = entranceDir;
        ExitDir = new Vector2Int(); 
    }
}
