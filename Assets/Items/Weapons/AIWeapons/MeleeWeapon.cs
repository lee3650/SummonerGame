using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IDamager
{
    [SerializeField] float Magnitude;
    [SerializeField] EventType EventType;
    [SerializeField] GameObject WielderObject;
    [SerializeField] float AttackStartToDamageDelay;

    IWielder Wielder;
    bool alreadyHit = false; 

    List<Event> UnmodifiedEventsToApplyOnHit = new List<Event>();
    List<Event> ModifiedEventsToApplyOnHit = new List<Event>();

    private void Awake()
    {
        UnmodifiedEventsToApplyOnHit.Add(new Event(EventType, Magnitude));
        Wielder = WielderObject.GetComponent<IWielder>();
    }

    public void AddAttackModifier(Event e)
    {
        UnmodifiedEventsToApplyOnHit.Add(e); //so, we actually want this to be in the unmodified list because that means it will be constant and consistent 
    }

    public void StartAttack(ITargetable target, AIAttackManager aIAttackManager)
    {
        ModifiedEventsToApplyOnHit = Wielder.ModifyEventList(UnmodifiedEventsToApplyOnHit); //this is a little messed up, because it is going to be passed in by reference, so we actually could modify the original, which we don't want to do. 
        //alreadyHit = false;
        StartCoroutine(HitTarget(target, aIAttackManager));
    }

    IEnumerator HitTarget(ITargetable target, AIAttackManager aIAttackManager)
    {
        yield return new WaitForSeconds(AttackStartToDamageDelay);
        if (aIAttackManager.IsTargetInRange(target))
        {
            HandleCollision(target);
        }
    }

    /*
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
     */

    protected virtual void HandleCollision(IEntity entity)
    {
        foreach (Event e in ModifiedEventsToApplyOnHit)
        {
            entity.HandleEvent(e);
        }
    }
}
