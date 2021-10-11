using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillCoating : Coating
{
    const float damageIncrease = 1.25f; 

    public override Event GetModifiedEvent(Event input)
    {
        return input;
    }

    public override float GetMoveSpeedAdjustment()
    {
        return 0.8f;
    }

    public override List<Event> ModifyAttackEvents(List<Event> original)
    {
        List<Event> result = new List<Event>();
        foreach (Event e in original)
        {
            result.Add(new Event(e.MyType, e.Magnitude * damageIncrease, e.Sender));
        }

        return result; 
    }

    public HillCoating(float timeLeft)
    {
        TimeLeft = timeLeft;
    }
}
