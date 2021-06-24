using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSearcher : MonoBehaviour
{
    [SerializeField] float SightRange; 
    [SerializeField] bool UsePrecedence;
    [Space(20)]
    [SerializeField] AIEntity MyEntity;
    [SerializeField] TargetManager TargetManager;
    [SerializeField] float SearchSpeed = 1f;

    private bool ShouldSearchForTarget = true;

    void Start()
    {
        StartCoroutine(SearchForTarget());
    }
    
    IEnumerator SearchForTarget()
    {
        //so, we'll use the closest enemy
        //or the highest precedence enemy who is closest. 

        while (ShouldSearchForTarget)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, SightRange);

            List<ILivingEntity> candidates = GetPossibleTargets(cols);

            ILivingEntity target = ChooseTargetFromColliders(candidates);
            
            if (target != null)
            {
                TargetManager.Target = target;
            }
            
            yield return new WaitForSeconds(SearchSpeed);
        }
    }

    List<ILivingEntity> GetPossibleTargets(Collider2D[] cols)
    {
        List<ILivingEntity> candidates = new List<ILivingEntity>();

        foreach (Collider2D col in cols)
        {
            ILivingEntity livingEntity;
            if (col.TryGetComponent<ILivingEntity>(out livingEntity))
            {
                if (livingEntity.IsAlive() && livingEntity.GetFaction() != MyEntity.GetFaction())
                {
                    candidates.Add(livingEntity);
                }
            }
        }

        return candidates;
    }

    //I'd rather this take a list of ILivingEntities in case later we switch over to having a database of all entities instead of collider searching 
    ILivingEntity ChooseTargetFromColliders(List<ILivingEntity> candidates)
    {
        if (candidates.Count == 0)
        {
            return null; //is that right? Hm. 
        }
        
        if (UsePrecedence)
        {
            candidates = GetHighestPrecedenceCandidates(candidates);
        }

        ILivingEntity result = candidates[0];
        float minDist = Vector2.Distance(transform.position, result.GetPosition());

        foreach (ILivingEntity e in candidates)
        {
            float dist = Vector2.Distance(transform.position, e.GetPosition());
            if (dist < minDist)
            {
                result = e;
                minDist = dist;
            }
        }

        return result; 
    }

    List<ILivingEntity> GetHighestPrecedenceCandidates(List<ILivingEntity> candidates)
    {
        List<ILivingEntity> result = new List<ILivingEntity>();

        //so... highest precedence is actually the lowest number, right? I think so. 

        int lowestNumber = int.MaxValue;

        foreach (ILivingEntity entity in candidates)
        {
            if (entity.GetPrecedence() < lowestNumber)
            {
                lowestNumber = entity.GetPrecedence();
            }
        }

        foreach (ILivingEntity entity in candidates)
        {
            if (entity.GetPrecedence() == lowestNumber)
            {
                result.Add(entity); //this is guaranteed to have at least one item, assuming the original list has at least one item. 
            }
        }

        return result; 
    }
}
