using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatRounder 
{
    public static float RoundFloat(float f, int places)
    {
        int intVersion = (int)f;
        if (f - intVersion < Mathf.Pow(10, -places))
        {
            return intVersion;
        }

        int ps = (int)(Mathf.Pow(10, places) * f);

        return ((float)ps / Mathf.Pow(10, places));
    }
}
