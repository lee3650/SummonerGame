using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamAttack : Projectile
{
    [SerializeField] float Lifespan; //probably like, 3? 
    [SerializeField] TargetManager TargetManager;
    [SerializeField] LineRenderer line;
    [SerializeField] TargetSearcher TargetSearcher;

    float timer = 0f;

    IWielder myWielder;
    IEntity Sender;
    //so, we're modifying our damage by Time.deltaTime, so basically it's not going to be super insane if you get a charm. 

    public override void Fire(IWielder wielder, IEntity sender)
    {
        timer = 0f;
        myWielder = wielder;
        TargetSearcher.Init();
        Sender = sender;

        EventsToApply.Add(new Event(EventType, Damage, sender));
        EventsToApply = wielder.ModifyEventList(EventsToApply);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (TargetManager.HasLivingTarget() && myWielder != null)
        {
            line.enabled = true;

            line.SetPositions(new Vector3[] {myWielder.GetTransform().position, TargetManager.Target.GetPosition()});

            foreach (Event e in EventsToApply)
            {
                print("Sending event!");
                TargetManager.Target.HandleEvent(Event.ScaleEvent(e, Time.deltaTime));
            }
        } else
        {
            line.enabled = false; 
        }

        if (timer >= Lifespan)
        {
            Destroy(gameObject); //okay, let's just try this. This seems a bit sketchy. 
        }
    }
}
