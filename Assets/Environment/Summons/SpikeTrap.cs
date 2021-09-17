using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour, IDamager
{
    [SerializeField] float Damage;
    [SerializeField] EventType EventType;
    private List<Event> EventsToApply = new List<Event>();

    private void Awake()
    {
        EventsToApply.Add(new Event(EventType, Damage));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IEntity entity; 
        if (collision.TryGetComponent<IEntity>(out entity))
        {
            foreach (Event e in EventsToApply)
            {
                entity.HandleEvent(e); 
            }
        }
    }

    public void AddAttackModifier(Event modifier)
    {
        EventsToApply.Add(modifier);
    }
}
