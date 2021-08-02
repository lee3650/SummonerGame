using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IDamager
{
    [SerializeField] Animator Animator;
    [SerializeField] AnimationClip Attack;
    [SerializeField] float Magnitude;
    [SerializeField] EventType EventType;

    bool alreadyHit = false; 

    List<Event> EventsToApplyOnHit = new List<Event>();

    private void Awake()
    {
        EventsToApplyOnHit.Add(new Event(EventType, Magnitude));
    }

    public void AddAttackModifier(Event e)
    {
        EventsToApplyOnHit.Add(e);
    }

    public void StartAttack()
    {
        alreadyHit = false; 
        Animator.Play(Attack.name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IEntity entity; 
        if (collision.TryGetComponent<IEntity>(out entity))
        {
            if (!alreadyHit)
            {
                alreadyHit = true; 
                HandleCollision(entity);
            }
        }
    }

    protected virtual void HandleCollision(IEntity entity)
    {
        foreach (Event e in EventsToApplyOnHit)
        {
            entity.HandleEvent(e);
        }
    }
}
