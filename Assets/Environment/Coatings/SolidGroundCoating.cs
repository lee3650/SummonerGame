using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidGroundCoating : Coating
{
    public override Event GetModifiedEvent(Event input)
    {
        /*
        switch (input.MyType)
        {
            case EventType.Fire:
            case EventType.Magic:
            case EventType.Physical:
                return new Event(input.MyType, input.Magnitude * 0.67f);
        }
         */
        return input;
    }

    public override float GetMoveSpeedAdjustment()
    {
        return 1f; //1.2f
    }

    public SolidGroundCoating(float timeLeft)
    {
        TimeLeft = timeLeft;
    }
}
