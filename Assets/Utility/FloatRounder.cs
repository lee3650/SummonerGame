using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatRounder 
{
    public static float RoundFloat(float f, int places)
    {
        float ps = (Mathf.Pow(10, places) * f);

        int rs = Mathf.RoundToInt(ps);

        return ((float)rs / Mathf.Pow(10, places));
    }
}
