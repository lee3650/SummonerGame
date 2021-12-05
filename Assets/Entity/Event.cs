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
        Recurrences = 0;
        Spreads = 0;
    }
    
    public Event (EventType type, float magnitude, IEntity sender, int recurrences, int spreads) : this(type, magnitude, sender, recurrences)
    {
        Spreads = spreads;
    }

    public Event(EventType type, float magnitude, IEntity sender, int recurrences) : this (type, magnitude, sender)
    {
        Recurrences = recurrences;
    }

    public static Event CopyEvent(Event e)
    {
        return new Event(e.MyType, e.Magnitude, e.Sender, e.Recurrences, e.Spreads);
    }

    public static Event ScaleEvent(Event e, float scale)
    {
        return new Event(e.MyType, e.Magnitude * scale, e.Sender, e.Recurrences, e.Spreads);
    }

    public int Spreads
    {
        get;
        set;
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

    public override string ToString()
    {
        return string.Format("Type: {0}, Recurrences: {1}, Spreads: {2}", MyType, Recurrences, Spreads);
    }
}
