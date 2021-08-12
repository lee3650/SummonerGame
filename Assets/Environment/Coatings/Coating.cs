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

    public abstract float GetMoveSpeedAdjustment();

    public abstract Event GetModifiedEvent(Event input);

    public virtual List<Event> ModifyAttackEvents(List<Event> original)
    {
        return original; 
    }

    public static Coating GetCoating(CoatingType coating, float timeLeft)
    {
        switch (coating)
        {
            case CoatingType.Water:
                return new WaterCoating(timeLeft);
            case CoatingType.Marsh:
                return new MarshCoating(timeLeft);
            case CoatingType.Hills:
                return new HillCoating(timeLeft);
            case CoatingType.SolidGround:
                return new SolidGroundCoating(timeLeft);
        }

        throw new System.Exception("Could not handle coating of type " + coating);
    }
}
