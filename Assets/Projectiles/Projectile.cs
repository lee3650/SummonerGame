using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IEntity
{
    [SerializeField] float Damage;
    [SerializeField] EventType EventType;
    [SerializeField] float Velocity;

    [SerializeField] bool StickToTarget;
    [SerializeField] MovementController MovementController;
    [SerializeField] Collider2D col;

    public void Fire()
    {
        MovementController.SetVelocity(transform.up, Velocity);
    }

    public void HandleEvent(Event e)
    {

    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //so, if it's an entity, we apply event
        //no matter what, we parent if StickToTarget is true. 

        Event myEvent = new Event(EventType, Damage);

        MovementController.SetVelocity(Vector2.zero, 0f);
        MovementController.DisableRigidbody();
        GetComponent<Rigidbody2D>().angularVelocity = 0f;

        IEntity entity;
        if (collision.transform.TryGetComponent<IEntity>(out entity))
        {
            entity.HandleEvent(myEvent);
        }

        if (StickToTarget)
        {
            col.enabled = false;
            transform.parent = collision.transform;
            StartCoroutine(Destroy());
        } 
        else
        {
            Destroy(gameObject);
        }
    }
    
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(15f);
        Destroy(gameObject);
    }
}
