using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSearcher : MonoBehaviour, IInitialize
{
    [SerializeField] float SightRange; 
    [SerializeField] bool UsePrecedence;
    [Space(20)]
    [SerializeField] AIEntity MyEntity;
    [SerializeField] TargetManager TargetManager;
    [SerializeField] float SearchSpeed = 1f;
    [Tooltip("Only used if MyEntity is null")]
    [SerializeField] Factions MyFaction; 

    private bool ShouldSearchForTarget = true;

    private SearchStates searchState = SearchStates.SearchForTarget;


    public void Init()
    {
        if (MyEntity != null)
        {
            MyFaction = MyEntity.GetFaction();
        }

        StartCoroutine(SearchForTarget());
    }

    IEnumerator SearchForTarget()
    {
        //so, we'll use the closest enemy
        //or the highest precedence enemy who is closest. 

        while (ShouldSearchForTarget)
        {
            switch (searchState)
            {
                case SearchStates.SearchForTarget:
                    SetTarget();
                    break;
                case SearchStates.AssignedTarget:
                    if (!TargetManager.IsTargetAlive())
                    {
                        searchState = SearchStates.SearchForTarget;
                    }
                    break;
            }
            
            yield return new WaitForSeconds(SearchSpeed);
        }
    }

    public void AssignTarget(ITargetable newTarget)
    {
        TargetManager.Target = newTarget;
        searchState = SearchStates.AssignedTarget;
    }

    void SetTarget()
    {
        List<ILivingEntity> livingEntities = TargetableEntitiesManager.GetTargetables();

        List<ILivingEntity> candidates = GetPossibleTargets(livingEntities);

        ILivingEntity target = ChooseTargetFromColliders(candidates);

        if (target != null && searchState == SearchStates.SearchForTarget)
        {
            TargetManager.Target = target;
        }
    }

    //shouldn't this go on TargetableEntitiesManager? 
    List<ILivingEntity> GetPossibleTargets(List<ILivingEntity> livingEntities)
    {
        List<ILivingEntity> candidates = new List<ILivingEntity>();

        foreach (ILivingEntity e in livingEntities)
        {
            if (ShouldAddCandidate(e))
            {
                candidates.Add(e);
            }
        }

        return candidates;
    }

    protected virtual bool ShouldAddCandidate(ILivingEntity e)
    {
        return e.CanBeTargeted() && e.GetFaction() != MyFaction;
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
            if (e != null)
            {
                float dist = Vector2.Distance(transform.position, e.GetPosition());
                if (dist < minDist)
                {
                    result = e;
                    minDist = dist;
                }
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

    enum SearchStates
    {
        SearchForTarget,
        AssignedTarget,
    }

    public void SetShouldSearchForTarget(bool val)
    {
        ShouldSearchForTarget = val; 
    }
}