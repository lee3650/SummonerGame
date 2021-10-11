using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event
{
    public Event(EventType type, float magnitude, IEntity sender)
    {
        MyType = type;
        Magnitude = magnitude;
        Sender = sender;
    }

    public EventType MyType
    {
        get;
    }
    public float Magnitude
    {
        get;
    }
    public IEntity Sender
    {
        get;
    }
}
