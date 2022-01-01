using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyEvent : MonoBehaviour, IDamager
{
    [SerializeField] EventType EventType;
    [SerializeField] float Magnitude;

    private List<Event> Events = new List<Event>();

    void Start()
    {
        Events.Add(new Event(EventType, Magnitude, null));
    }

    public void AddAttackModifier(Event e)
    {
        Events.Add(e);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ILivingEntity entity; 
        if (collision.TryGetComponent<ILivingEntity>(out entity))
        {
            if (VectorRounder.RoundVector(transform.position) == VectorRounder.RoundVector(entity.GetPosition()))
            {
                foreach (Event e in Events)
                {
                    entity.HandleEvent(Event.ScaleEvent(e, Time.deltaTime));
                }
            }
        }
    }
}
