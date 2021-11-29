using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadEvents : MonoBehaviour, ISubEntity
{
    [SerializeField] Factions TargetFaction;
    const float SpreadRange = 2f;

    public Event ModifyEvent(Event e)
    {
        return e;
    }

    public void HandleEvent(Event e)
    {
        if (e.Spreads <= 0)
        {
            return;
        } 

        List<ILivingEntity> targets = TargetableEntitiesManager.GetTargets(TargetFaction, 0); //I guess zero is the minimum? 

        foreach (ILivingEntity t in targets)
        {
            HandleRecurringEvent recur = t.GetTransform().GetComponent<HandleRecurringEvent>();
            if (recur != null)
            {
                if (t != GetComponent<ILivingEntity>() && !recur.EventRecurs(e.MyType) && Vector2.Distance(t.GetPosition(), transform.position) < SpreadRange)
                {
                    e.Spreads--;
                    t.HandleEvent(e);
                    if (e.Spreads <= 0)
                    {
                        return;
                    }
                }
            }
        }
    }
}
