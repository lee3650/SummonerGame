using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarshCoating : Coating
{
    public override Event GetModifiedEvent(Event input)
    {
        return input;
    }

    public override float GetMoveSpeedAdjustment()
    {
        return 0.67f;
    }

    public MarshCoating(float timeLeft)
    {
        TimeLeft = timeLeft;
    }
}
