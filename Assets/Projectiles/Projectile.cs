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

    [SerializeField] bool CheckFaction;
    [SerializeField] Factions TargetFaction; 

    bool alreadyHit = false; 

    protected List<Event> EventsToApply = new List<Event>();

    public virtual void Fire(IWielder wielder, IEntity sender)
    {
        EventsToApply.Add(new Event(EventType, Damage, sender));
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

        bool applyEvents = true;

        if (CheckFaction)
        {
            ILivingEntity e;
            if (collision.transform.TryGetComponent<ILivingEntity>(out e))
            {
                if (e.GetFaction() != TargetFaction)
                {
                    applyEvents = false;
                }
            } else
            {
                //so, this is a bit sketchy, but if it's not a living entity we don't want to damage it then
                applyEvents = false; 
            }
        }

        if (applyEvents)
        {
            IEntity entity;
            if (collision.transform.TryGetComponent<IEntity>(out entity))
            {

                foreach (Event e in EventsToApply)
                {
                    entity.HandleEvent(e);
                }
            }
        }

        OnHit();

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
    
    protected virtual void OnHit()
    {

    }

    public void Rotate(float amount)
    {
        transform.eulerAngles = new Vector3(0f, 0f, amount);
        MovementController.SetVelocity(transform.up, Velocity);
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }
}
