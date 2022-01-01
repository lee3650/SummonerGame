using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritType : MonoBehaviour, ISubEntity
{
    [SerializeField] List<EventType> TypesToCrit;

    public Event ModifyEvent(Event e)
    {
        if (TypesToCrit.Contains(e.MyType))
        {
            CritGraphicPool.ShowCrit(transform.position + new Vector3(0, 1, 0));
            return Event.ScaleEvent(e, 2f);
        }
        return e; 
    }

    public void HandleEvent(Event e)
    {

    }
}
