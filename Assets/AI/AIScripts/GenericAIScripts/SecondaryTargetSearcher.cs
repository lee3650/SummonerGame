using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryTargetSearcher : MonoBehaviour, IInitialize
{
    [SerializeField] float SearchRange = 1.75f;
    [SerializeField] float SearchFrequency = 0.25f;
    [SerializeField] TargetSearcher TargetSearcher;
    [SerializeField] Factions TargetFaction = Factions.Player;

    private ITargetable previousTarget = null; //technically we could subscribe to their death event... that'd be interesting. I don't think that'd be any faster, and I'd need a new field, so. 

    public void Init()
    {
        StartCoroutine(SearchForSecondaryTargetOverTime());
    }

    private IEnumerator SearchForSecondaryTargetOverTime()
    {
        while (true)
        {
            if (previousTarget == null || !previousTarget.IsAlive())
            {
                previousTarget = SearchForSecondaryTarget(transform.position);
                TryAssignTarget();
            }
            yield return new WaitForSeconds(SearchFrequency);
        }
    }

    //that'd be cool if we had a stack of targets. 
    protected void TryAssignTarget()
    {
        if (previousTarget != null)
        {
            TargetSearcher.AssignTarget(previousTarget);
        }
    }

    protected virtual ILivingEntity SearchForSecondaryTarget(Vector2 position)
    {
        return SearchForTargetAdjacent(position);
    }

    protected virtual bool IncludeTarget(ILivingEntity target)
    {
        return target.IsAlive(); 
    }

    protected virtual void RecalculateSecondaryTargetNow(Vector2 position)
    {
        previousTarget = SearchForTargetAdjacent(position); //okay. So, this only assigns state. It doesn't return... hm. 
        TryAssignTarget();
    }

    protected ILivingEntity SearchForTargetAdjacent(Vector2 position)
    {
        List<ILivingEntity> targets = TargetableEntitiesManager.GetTargets(TargetFaction, 0); //hm... I guess that's okay. That's some pretty clearly duplicated knowledge though. 

        for (int i = targets.Count - 1; i >= 0; i--)
        {
            if (!IncludeTarget(targets[i]))
            {
                targets.RemoveAt(i);
            }
        }

        for (int i = 0; i < targets.Count; i++)
        {
            if (Vector2.Distance(targets[i].GetPosition(), position) < SearchRange)
            {
                return targets[i];
            }
        }

        return null;
    }

    private void Update()
    {
        if (previousTarget != null)
        {
            if (!previousTarget.IsAlive())
            {
                RecalculateSecondaryTargetNow(transform.position); //
            }
        }
    }
}
