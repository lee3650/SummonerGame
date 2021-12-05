using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flames : Projectile
{
    [SerializeField] float lifespan;
    [SerializeField] int Spreads;
    [SerializeField] int Recurrences;

    float timer = 0f;

    public override void Fire(IWielder wielder, IEntity sender)
    {
        EventsToApply.Add(new Event(EventType, Damage, sender, Recurrences, Spreads));
        EventsToApply = wielder.ModifyEventList(EventsToApply);
        
        timer = 0f;
    }

    private void Update()
    {
        //so, starting at our position, we want to basically boxcast in area of attack
        //and any entity we find in there we apply the fire event to
        //okay... that means we should let arrows catch on fire and spread fire
        //that'd be so cool... 
        //might get a bit op though lol 
        //wait, so this would just be a bonfire then but stronger lol 

        timer += Time.deltaTime;

        if (timer > lifespan)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("trigger entered: " + collision.name);
        IEntity e;
        if (collision.TryGetComponent<IEntity>(out e))
        {
            print("found entity!");
            foreach (Event ev in EventsToApply)
            {
                print("Applying event! " + ev.MyType + ", mag: " + ev.Magnitude);
                print("Applying event to: " + e.GetTransform().name);
                e.HandleEvent(Event.CopyEvent(ev));
            }
        }
    }
}
