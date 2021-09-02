using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyRangedPursuitState : AIPursuitState
{
    [SerializeField] SightChecker SightChecker;
    [SerializeField] int RangeAdjustment = 0; 

    public override bool ShouldMoveAtTarget()
    {
        bool result = base.ShouldMoveAtTarget() || (!SightChecker.NoUnbreakableWallsInWay(TargetManager.Target.GetPosition())) || AIAttackManager.CanAttack(TargetManager.Target) == false;
        
        return result;
    }

    int GetChangeInWallScore(Vector2 direction, int dist, Vector2 targetPos)
    {
        TileType t = MapManager.GetTileType((direction * dist) + targetPos);

        int delta = 0;

        if (t == TileType.Wall)
        {
            delta = 1000;
        }
        if (t == TileType.BreakableWall || t == TileType.Gate)
        {
            delta++;
        }

        return delta; 
    }

    protected override Vector2 GetPathfindGoal()
    {
        Vector2 targetPos = VectorRounder.RoundVector(TargetManager.Target.GetPosition());

        int maxDist = (int)AIAttackManager.GetRange() - RangeAdjustment;

        List<Vector2Dist> candidates = new List<Vector2Dist>();

        int wallScorePlusX = 0;
        int wallScoreMinusY = 0;
        int wallScorePlusY = 0;

        float distToTarget = Vector2.Distance(transform.position, targetPos);

        for (int d = maxDist; d >= 0; d--)
        {
            wallScorePlusX += GetChangeInWallScore(new Vector2(1, 0), d, targetPos);
            wallScorePlusY += GetChangeInWallScore(new Vector2(0, 1), d, targetPos);
            wallScoreMinusY += GetChangeInWallScore(new Vector2(0, -1), d, targetPos);
        }

        for (int i = maxDist; i >= 0; i--)
        {
            Vector2 candidate = targetPos + new Vector2(i, 0);
            TryAddCandidate(candidate, candidates, wallScorePlusX, distToTarget);

            candidate = targetPos + new Vector2(0, i);
            TryAddCandidate(candidate, candidates, wallScorePlusY, distToTarget);

            candidate = targetPos + new Vector2(0, -i);
            TryAddCandidate(candidate, candidates, wallScoreMinusY, distToTarget);
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

    void TryAddCandidate(Vector2 candidate, List<Vector2Dist> candidates, int wallScore, float targetDist)
    {
        if (MapManager.IsPointTraversable(candidate, true))
        {
            candidates.Add(new Vector2Dist(candidate, Vector2.Distance(transform.position, candidate) / targetDist, wallScore));
        }
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
