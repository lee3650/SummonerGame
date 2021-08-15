using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPurchasable
{
    string GetDescription();
    float GetCost();
    float GetRecurringCost();
}
