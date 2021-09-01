using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedPursuitState : AIPursuitState
{
    [SerializeField] SightChecker SightChecker;
    [SerializeField] int RangeAdjustment = 0; 

    public override bool ShouldMoveAtTarget()
    {
        bool result = base.ShouldMoveAtTarget() || (!SightChecker.NoUnbreakableWallsInWay(TargetManager.Target.GetPosition())) || AIAttackManager.CanAttack(TargetManager.Target) == false;
        
        return result;
    }

    protected override Vector2 GetPathfindGoal()
    {
        Vector2 targetPos = VectorRounder.RoundVector(TargetManager.Target.GetPosition());

        int maxDist = (int)AIAttackManager.GetRange() - RangeAdjustment;

        List<Vector2Dist> candidates = new List<Vector2Dist>();

        for (int i = maxDist; i >= 0; i--)
        {
            Vector2 candidate = targetPos + new Vector2(i, 0);
            if (MapManager.IsPointTraversable(candidate, true))
            {
                candidates.Add(new Vector2Dist(candidate, Vector2.Distance(transform.position, candidate)));
            }

            candidate = targetPos + new Vector2(0, i);

            if (MapManager.IsPointTraversable(targetPos + new Vector2(0, i), true))
            {
                candidates.Add(new Vector2Dist(candidate, Vector2.Distance(transform.position, candidate)));
            }
            
            candidate = targetPos + new Vector2(0, -i);

            if (MapManager.IsPointTraversable(targetPos + new Vector2(0, -i), true))
            {
                candidates.Add(new Vector2Dist(candidate, Vector2.Distance(transform.position, candidate)));
            }
        }

        Vector2Dist result = new Vector2Dist(targetPos, 100000f);

        foreach (Vector2Dist cand in candidates)
        {
            if (cand.Distance < result.Distance)
            {
                result = cand; 
            }
        }

        return result.Point;
    }

    struct Vector2Dist
    {
        public Vector2 Point { get; }
        public float Distance { get; }

        public Vector2Dist(Vector2 v, float d)
        {
            Distance = d;
            Point = v; 
        }
    }
}
