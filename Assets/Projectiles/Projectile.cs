using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IEntity, IDamager
{
    [SerializeField] float Damage;
    [SerializeField] EventType EventType;
    [SerializeField] float Velocity;

    [SerializeField] bool StickToTarget;
    [SerializeField] MovementController MovementController;
    [SerializeField] Collider2D col;

    bool alreadyHit = false; 

    List<Event> EventsToApply = new List<Event>();

    private void Awake()
    {
        EventsToApply.Add(new Event(EventType, Damage));
    }

    public void Fire(IWielder wielder)
    {
        EventsToApply = wielder.ModifyEventList(EventsToApply);
        MovementController.SetVelocity(transform.up, Velocity);
    }

    public void HandleEvent(Event e)
    {
        
    }

    public void AddAttackModifier(Event e)
    {
        EventsToApply.Add(e);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //so, if it's an entity, we apply event
        //no matter what, we parent if StickToTarget is true. 
        if (alreadyHit)
        {
            return; 
        } else
        {
            alreadyHit = true;
        }

        MovementController.SetVelocity(Vector2.zero, 0f);
        MovementController.DisableRigidbody();
        GetComponent<Rigidbody2D>().angularVelocity = 0f;

        IEntity entity;
        if (collision.transform.TryGetComponent<IEntity>(out entity))
        {
            foreach (Event e in EventsToApply)
            {
                entity.HandleEvent(e);
            }
        }

        if (StickToTarget)
        {
            col.enabled = false;
            transform.parent = collision.transform;
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(Destroy());
            } 
        } 
        else
        {
            Destroy(gameObject);
        }
    }
    
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }
}
