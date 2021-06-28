using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnergyManager
{
    float GetRemainingPercentage();

    float GetMax();

    float GetCurrent();
}
