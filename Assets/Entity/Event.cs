using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event
{
    public Event(EventType type, float magnitude)
    {
        MyType = type;
        Magnitude = magnitude;
    }

    public EventType MyType
    {
        get;
    }
    public float Magnitude
    {
        get;
    }
}
