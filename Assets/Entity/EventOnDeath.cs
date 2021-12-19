using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventOnDeath : MonoBehaviour
{
    [SerializeField] HealthManager HealthManager;
    [SerializeField] float Radius;
    [SerializeField] float Magnitude;
    [SerializeField] EventType EventType;
    [SerializeField] int Recurs;
    [SerializeField] int Spreads; 

    private void Awake()
    {
        HealthManager.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        List<ILivingEntity> targets = TargetableEntitiesManager.GetAllTargets(transform.position, Radius);

        foreach (ILivingEntity t in targets)
        {
            if (t.IsAlive()) 
            {
                t.HandleEvent(new Event(EventType, Magnitude, null, Recurs, Spreads));
            }
        }
    }
}
