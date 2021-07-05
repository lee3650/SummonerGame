using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    [SerializeField] float Damage;
    [SerializeField] EventType EventType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IEntity e; 
        if (collision.TryGetComponent<IEntity>(out e))
        {
            e.HandleEvent(new Event(EventType, Damage));
            Destroy(gameObject);
        }
    }
}
