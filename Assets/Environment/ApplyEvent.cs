using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyEvent : MonoBehaviour
{
    [SerializeField] EventType EventType;
    [SerializeField] float Magnitude; 
    private void OnTriggerStay2D(Collider2D collision)
    {
        ILivingEntity entity; 
        if (collision.TryGetComponent<ILivingEntity>(out entity))
        {
            if (VectorRounder.RoundVector(transform.position) == VectorRounder.RoundVector(entity.GetPosition()))
            {
                entity.HandleEvent(new Event(EventType, Magnitude));
            }
        }
    }
}
