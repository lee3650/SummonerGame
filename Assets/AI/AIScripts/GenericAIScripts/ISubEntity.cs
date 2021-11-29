using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubEntity
{
    Event ModifyEvent(Event e);
    void HandleEvent(Event e);
}
