﻿using System.Collections;
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
                return new Event(input.MyType, input.Magnitude * 2f); 
        }
        return input; 
    }
    
    public WaterCoating(float timeLeft)
    {
        TimeLeft = timeLeft;
    }
}
