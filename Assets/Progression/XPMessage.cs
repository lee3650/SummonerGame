using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPMessage
{
    public string Message
    {
        get;
    }

    public float XpGain
    {
        get;
    }

    public XPMessage(string message, float xpGain)
    {
        Message = message;
        XpGain = xpGain;
    }

    public string CompoundMessage()
    {
        return Message + ": " + XpGain + " XP";
    }
}
