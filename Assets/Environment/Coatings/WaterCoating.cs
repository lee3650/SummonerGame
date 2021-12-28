using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCoating : Coating
{
    public override Event GetModifiedEvent(Event input)
    {
        switch (input.MyType)
        {
            case EventType.Fire:
            case EventType.Magic:
            case EventType.Physical:
                return new Event(input.MyType, input.Magnitude * 1.5f, input.Sender, input.Recurrences, input.Spreads); 
        }
        return input; 
    }
    
    public override float GetMoveSpeedAdjustment()
    {
        return 1f; 
    }

    public WaterCoating(float timeLeft)
    {
        TimeLeft = timeLeft;
    }
}
