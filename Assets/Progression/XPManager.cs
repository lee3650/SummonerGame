using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    private static bool exitingLevel = false;
    private static List<XPMessage> XPMessages = new List<XPMessage>();

    public static void ResetState()
    {
        ResetXPMessages();
        exitingLevel = false;
    }

    public static List<XPMessage> GetXPMessages()
    {
        return XPMessages;
    } 

    public static void AddXPMessage(XPMessage message)
    {
        XPMessages.Add(message);
    }

    public static bool ExitingLevel
    {
        get
        {
            return exitingLevel;
        }
        set
        {
            exitingLevel = value;
        }
    }

    public static void ResetXPMessages()
    {
        XPMessages = new List<XPMessage>();
    }
}
