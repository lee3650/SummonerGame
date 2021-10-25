using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryTargetSearcher : MonoBehaviour, IInitialize
{
    [SerializeField] float SearchRange = 1.75f;
    [SerializeField] float SearchFrequency = 0.25f;
    [SerializeField] TargetSearcher TargetSearcher;
    [SerializeField] Factions TargetFaction = Factions.Player;

    private Stack<ITargetable> secondaryTargets = new Stack<ITargetable>();

    public void Init()
    {
        StartCoroutine(SearchForSecondaryTargetOverTime());
    }

    public void SetSecondaryTarget(ITargetable secondaryTarget)
    {
        secondaryTargets.Push(secondaryTarget);
        TargetSearcher.AssignTarget(secondaryTarget);
    }

    private IEnumerator SearchForSecondaryTargetOverTime()
    {
        while (true)
        {
            if (IsCurrentTargetDead())
            {
                secondaryTargets.Pop();
                if (secondaryTargets.Count != 0)
                {
                    TargetSearcher.AssignTarget(secondaryTargets.Peek());
                }
            }

            if (secondaryTargets.Count == 0)
            {
                ITargetable target = SearchForSecondaryTarget(transform.position);
                if (target != null)
                {
                    SetSecondaryTarget(target);
                }
            }

            yield return new WaitForSeconds(SearchFrequency);
        }
    }

    private bool IsCurrentTargetDead()
    {
        return secondaryTargets.Count > 0 && !secondaryTargets.Peek().IsAlive();
    }

    protected virtual ILivingEntity SearchForSecondaryTarget(Vector2 position)
    {
        return SearchForTargetAdjacent(position);
    }

    protected virtual bool IncludeTarget(ILivingEntity target)
    {
        return target.IsAlive(); 
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
            float targetDist = Vector2.Distance(targets[i].GetPosition(), position);
            if (targetDist < SearchRange)
            {
                return targets[i];
            } 
        }

        return null;
    }

    /*
    protected virtual void RecalculateSecondaryTargetNow(Vector2 position)
    {
        ITargetable target = SearchForTargetAdjacent(position); //okay. So, this only assigns state. It doesn't return... hm. 
        if (target != null)
        {
            SetSecondaryTarget(target);
        }
    }
    
    private void Update()
    {
        if (secondaryTargets.Count != 0)
        {
            if (!secondaryTargets.Peek().IsAlive())
            {
                RecalculateSecondaryTargetNow(transform.position); 
            }
        }

        if (GetComponent<AIEntity>().IsAlive() == false)
        {
            Destroy(this); //for testing purposes
        }
    }
     */
}
