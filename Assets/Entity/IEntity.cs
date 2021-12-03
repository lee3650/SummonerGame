using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    void HandleEvent(Event e);

    Transform GetTransform();
}
