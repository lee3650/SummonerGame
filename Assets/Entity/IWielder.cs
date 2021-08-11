using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWielder
{
    void OnHit(IEntity hit);
    List<Event> ModifyEventList(List<Event> unmodifiedList);
}
