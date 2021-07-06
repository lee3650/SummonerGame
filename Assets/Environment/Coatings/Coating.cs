using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Coating
{
    public float TimeLeft
    {
        get;
        set;
    }

    public abstract Event GetModifiedEvent(Event input);

    public static Coating GetCoating(CoatingType coating, float timeLeft)
    {
        switch (coating)
        {
            case CoatingType.Water:
                return new WaterCoating(timeLeft);

        }
        throw new System.Exception("Could not handle coating of type " + coating);
    }
}
