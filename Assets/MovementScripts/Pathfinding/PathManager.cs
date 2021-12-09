using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour, IWaveNotifier, IResettable
{
    private static Vector2Int[] dirs = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(1, 1),
        new Vector2Int(-1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, -1),
    };

    public static bool ResetPaths
    {
        get
        {
            return resetPaths;
        }
        set
        {
            if (!WaveSpawner.IsCurrentWaveDefeated)
            {
                resetPaths = value; 
            }
        }
    }

    public void ResetState()
    {
        resetPaths = false;
        StartToPath = new Dictionary<Vector2Int, SearchNode>();
    }

    private static bool resetPaths = false; 

    float WallTimer = 0f;
    const float WallResetTime = 1f;

    private static Dictionary<Vector2Int, SearchNode> StartToPath = new Dictionary<Vector2Int, SearchNode>(); 

    private void Awake()
    {
        StartToPath = new Dictionary<Vector2Int, SearchNode>();
        WaveSpawner.NotifyWhenWaveEnds(this);
    }

    public void OnWaveEnds()
    {
        StartToPath = new Dictionary<Vector2Int, SearchNode>();
        WallTimer = 0f;
        resetPaths = false; 
    }

    void Update()
    {
        if (!WaveSpawner.IsCurrentWaveDefeated)
        {
            WallTimer += Time.deltaTime;
            if (WallTimer > WallResetTime)
            {
                WallTimer = 0f;
                if (ResetPaths)
                {
                    StartToPath = new Dictionary<Vector2Int, SearchNode>();
                }
            }
        }
    }

    public static Vector2Int HomeTile
    {
        get;
        set;
    }

    public static SearchNode GetLatestPath(Vector2Int start)
    {
        SearchNode path;
        if  (StartToPath.TryGetValue(start, out path))
        {
            return path; 
        } 
        else
        {
            print("Path manager calculating path to target: " + start);
            StartToPath[start] = FindPath(start);
            return StartToPath[start];
        }
    }

    public static void DrawPath(Vector2Int startLoc)
    {
        SearchNode path = FindPath(startLoc);

        while (path != null)
        {
            Debug.DrawLine((Vector2)startLoc, (Vector2)path.Pos, Color.red, 10f);
            path = path.ParentNode;
        }
    }

    private static SearchNode FindPath(Vector2Int s)
    {
        Dictionary<Vector2Int, float> closedList = new Dictionary<Vector2Int, float>();

        SearchNode start = new SearchNode(HomeTile, null);

        start.g = 0;
        start.h = 0;
        start.f = 0;
        
        PathNodeHeap heap = new PathNodeHeap();
        heap.Insert(start);

        int iterations = 0;

        while (!heap.IsEmpty() && iterations < 1000)
        {
            SearchNode min = heap.DeleteMin(); //get the closest one 

            iterations++;

            if (closedList.TryGetValue(min.Pos, out float f)) 
            {
                if (min.f < f)
                {
                    closedList[min.Pos] = min.f;
                }
            }
            else
            {
                closedList[min.Pos] = min.f;
            }

            for (int i = 0; i < 8; i++)
            {
                Vector2Int testPos = min.Pos + dirs[i];

                bool trav = MapManager.IsPointTraversable(testPos, true);
                float travCost = 10000f;

                if (trav)
                {
                    travCost = MapManager.GetTraversalCost(testPos.x, testPos.y);
                }

                if (i >= 4 && trav) //4-7, indices, inclusive, are diagonal 
                {
                    Vector2Int p1 = min.Pos + new Vector2Int(dirs[i].x, 0);
                    Vector2Int p2 = min.Pos + new Vector2Int(0, dirs[i].y);
                    trav = MapManager.IsPointTraversable(p1, true) && MapManager.IsPointTraversable(p2, true);
                    if (trav)
                    {
                        travCost = Mathf.Max(MapManager.GetTraversalCost(p1.x, p1.y), MapManager.GetTraversalCost(p2.x, p2.y), travCost);
                        travCost *= 1.414f; //sqrt 2 -> if we move diagonally, we have to add some movement cost to that 
                    }
                }

                if (trav)
                {
                    SearchNode node = new SearchNode(testPos, min);
                    node.g = min.g + travCost;
                    node.h = GetHToGoal(testPos, s);
                    node.f = node.g + node.h;

                    if (node.Pos == s)
                    {
                        return node; 
                    }

                    bool skip = false;

                    if (closedList.TryGetValue(node.Pos, out float mf))
                    {
                        if (mf <= node.f) //yeah so we need the equals in there, basically. 
                        {
                            skip = true;
                        }
                    }
                     

                    if (!skip)
                    {
                        List<SearchNode> points = heap.NodesAtPoint(testPos);
                        foreach (SearchNode n in points)
                        {
                            if (n.f <= node.f) //need the equals, otherwise we'll repeat nodes
                            {
                                skip = true;
                                break;
                            }
                        }
                    }

                    if (!skip)
                    {
                        heap.Insert(node);
                    }
                }
            }
        }

        print("failed! Iterations: " + iterations);
        print("failed! Closed list count: " + closedList.Values.Count);

        throw new System.Exception("Pathfinding to the home tile failed!");
    }

    private static float GetHToGoal(Vector2Int pos, Vector2Int end)
    {
        return Mathf.Abs(pos.x - end.x) + Mathf.Abs(pos.y - end.y);
    }
}

public class PathNode
{
    public Vector2Int Pos;
    public PathNode Parent;
    public float f;
    public float h;
    public float g;

    public PathNode ClosedNext;

    public PathNode(Vector2Int pos, PathNode parent)
    {
        Pos = pos;
        Parent = parent; 
    }
}
