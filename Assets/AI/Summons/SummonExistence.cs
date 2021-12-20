using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonExistence : MonoBehaviour, IResettable
{
    private static Dictionary<SummonType, int> TypeToCount = new Dictionary<SummonType, int>();

    public static void IncrementCount(SummonType type)
    {
        if (TypeToCount.TryGetValue(type, out int num))
        {
            TypeToCount[type]++;
        } else
        {
            TypeToCount[type] = 1;
        }
    }

    public static void DecrementCount(SummonType type)
    {
        if (TypeToCount.TryGetValue(type, out int num))
        {
            TypeToCount[type]--;
        }
        else
        {
            throw new System.Exception("Decremented summon type that doesn't exist!");
        }
    }

    public static bool TypeDoesNotExist(SummonType type)
    {
        if (TypeToCount.TryGetValue(type, out int num))
        {
            print("num for type " + type + " is " + num);
            return num == 0;
        }
        return true; 
    }

    public void ResetState()
    {
        TypeToCount = new Dictionary<SummonType, int>();
    }
}
