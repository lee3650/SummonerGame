using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWielder
{
    Transform GetTransform();
    void OnHit(IEntity hit);
    List<Event> ModifyEventList(List<Event> unmodifiedList);
}
