using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSurroundingFriendlies : MonoBehaviour
{
    [SerializeField] HealthManager hm;
    [SerializeField] Factions Faction;
    [SerializeField] float HealAmount;
    [SerializeField] int maxHeals;
    [SerializeField] float HealRange; 
    [SerializeField] AIEntity Sender; 
    
    private void Awake()
    {
        hm.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        List<ILivingEntity> friendlies = TargetableEntitiesManager.GetTargets(Faction, 0);

        List<ILivingEntity> inRange = new List<ILivingEntity>();

        int heals = maxHeals;
        foreach (ILivingEntity e in friendlies)
        {
            if (e != Sender && InRange(e.GetPosition()) && e.IsAlive())
            {
                inRange.Add(e);
                heals--;
                if (heals == 0)
                {
                    break;
                }
            }
        }

        foreach (ILivingEntity e in inRange)
        {
            e.HandleEvent(new Event(EventType.Heal, HealAmount, Sender));
            CritGraphicPool.ShowHeal(transform.position);
        }
    }

    private bool InRange(Vector2 pos)
    {
        return (pos - (Vector2)transform.position).sqrMagnitude < HealRange;
    }
}
