using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatRounder 
{
    public static float RoundFloat(float f, int places)
    {
        /*
        int intVersion = (int)f;
        if (f - intVersion < Mathf.Pow(10, -places))
        {
            return intVersion;
        }
         */

        float ps = (Mathf.Pow(10, places) * f);

        int rs = Mathf.RoundToInt(ps);

        return ((float)ps / Mathf.Pow(10, places));
    }
}
