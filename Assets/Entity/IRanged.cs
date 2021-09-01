using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRanged 
{
    float GetRange();
    bool IsCrossShaped();
    float GetCrossDelta();
}
