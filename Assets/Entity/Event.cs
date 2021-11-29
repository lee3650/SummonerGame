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
        Recurrences = 1;
    }
    
    public Event(EventType type, float magnitude, IEntity sender, int recurrences) : this (type, magnitude, sender)
    {
        Recurrences = recurrences;
    }

    public static Event CopyEvent(Event e)
    {
        return new Event(e.MyType, e.Magnitude, e.Sender, e.Recurrences);
    }

    public int Recurrences
    {
        get;
        set;
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
