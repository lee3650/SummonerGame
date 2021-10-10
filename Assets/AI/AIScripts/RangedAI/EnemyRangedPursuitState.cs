using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyRangedPursuitState : AIPursuitState
{
    [SerializeField] SightChecker SightChecker;
    [SerializeField] int RangeAdjustment = 0;
    float distanceWeight = 1.5f;

    public override bool ShouldMoveAtTarget()
    {
        bool result = base.ShouldMoveAtTarget() || AIAttackManager.CanAttack(TargetManager.Target) == false; //|| (!SightChecker.NoUnbreakableWallsInWay(TargetManager.Target.GetPosition()))

        return result;
    }

    Dictionary<Vector2Int, Vector2Int> FindFurthestPoints(Vector2Int targetPos, int maxDist, List<Vector2Int> dirs)
    {
        Dictionary<Vector2Int, Vector2Int> result = new Dictionary<Vector2Int, Vector2Int>();

        foreach (Vector2Int dir in dirs)
        {
            result[dir] = CalculateFurthestPoint(targetPos, maxDist, dir);
        }

        return result;
    }

    Vector2Int CalculateFurthestPoint(Vector2Int targetPos, int maxDist, Vector2Int dir)
    {
        Vector2Int result = targetPos;

        for (int i = 0; i <= maxDist; i++)
        {
            //it's okay to start at 0 because the initial point can't be untraversable 
            if (MapManager.IsPointInBounds(result.x + dir.x, result.y + dir.y) == false)
            {
                break; 
            }
            if (MapManager.GetTileType(result + dir) != TileType.Wall && MapManager.GetTileType(result + dir) != TileType.Valley)
            {
                result += dir;
            } else
            {
                break;
            }
        }

        return result; 
    }

    Dictionary<Vector2Int, int> GetWallScores(List<Vector2Int> dirs, Dictionary<Vector2Int, Vector2Int> furthestPoints, Vector2Int startPoint)
    {
        Dictionary<Vector2Int, int> result = new Dictionary<Vector2Int, int>();

        foreach (Vector2Int dir in dirs)
        {
            result[dir] = CalculateWallScore(dir, furthestPoints[dir], startPoint);
        }

        return result; 
    }

    int CalculateWallScore(Vector2Int dir, Vector2Int furthestPoint, Vector2Int startPoint)
    {
        Vector2Int cur = startPoint;
        int result = 0;

        while (Vector2.Distance(cur, startPoint) <= Vector2.Distance(furthestPoint, startPoint))
        {
            TileType t = MapManager.GetTileType(cur);
            if (t == TileType.BreakableWall || t == TileType.Gate)
            {
                result += 1; 
            }
            if (t == TileType.Wall)
            {
                throw new System.Exception("There was an unbreakable wall in the way!");
            }

            cur += dir; 
        }

        return result; 
    }

    protected override Vector2 GetPathfindGoal()
    {
        Vector2Int targetPos = VectorRounder.RoundVectorToInt(TargetManager.Target.GetPosition());

        int maxDist = (int)AIAttackManager.GetRange() - RangeAdjustment;

        List<Vector2Dist> candidates = new List<Vector2Dist>();

        List<Vector2Int> dirs = new List<Vector2Int>()
        {
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 0)
        };

        float distToTarget = Vector2.Distance(transform.position, targetPos);

        Dictionary<Vector2Int, Vector2Int> furthestPoints = FindFurthestPoints(targetPos, maxDist, dirs);
        Dictionary<Vector2Int, int> DirToWallScore = GetWallScores(dirs, furthestPoints, targetPos);

        foreach (KeyValuePair<Vector2Int, int> pairs in DirToWallScore)
        {
            print(string.Format("Direction: {0}, wall score: {1}", pairs.Key, pairs.Value));
        }

        Vector2 candidate;
        for (int i = 0; i <= maxDist; i++)
        {
            foreach (Vector2Int dir in dirs)
            {
                candidate = targetPos + dir;
                TryAddCandidate(candidate, candidates, DirToWallScore[dir], distToTarget, furthestPoints[dir], targetPos);
            }
        }

        Vector2Dist result = new Vector2Dist(targetPos, 100000f, 100000);

        foreach (Vector2Dist cand in candidates)
        {
            if (cand.Score < result.Score)
            {
                result = cand; 
            }
        }

        return result.Point;
    }

    void TryAddCandidate(Vector2 candidate, List<Vector2Dist> candidates, int wallScore, float targetDist, Vector2Int furthestPoint, Vector2 target)
    {
        if (MapManager.IsPointTraversable(candidate, true) && WithinFurthestPoint(candidate, furthestPoint, target))
        {
            candidates.Add(new Vector2Dist(candidate, distanceWeight * Vector2.Distance(transform.position, candidate) / targetDist, wallScore));
        }
    }

    bool WithinFurthestPoint(Vector2 candidate, Vector2Int furthestPoint, Vector2 target)
    {
        if (Vector2.Distance(target, furthestPoint) >= Vector2.Distance(candidate, target))
        {
            return true; 
        }
        return false; 
    }

    struct Vector2Dist
    {
        public Vector2 Point { get; }
        public float DistanceScore { get; }

        public float Score { get; }

        public Vector2Dist(Vector2 v, float d, int wallScore)
        {
            DistanceScore = d;
            Point = v;
            Score = DistanceScore + wallScore;
        }
    }
}
