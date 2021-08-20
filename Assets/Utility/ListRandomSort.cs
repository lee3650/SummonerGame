using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListRandomSort<T>
{
    public static IList<T> SortListRandomly(IList<T> ts)
    {
        for (int i = 0; i < ts.Count; i++)
        {
            int swapIndex = Random.Range(i, ts.Count);
            T swap = ts[swapIndex];
            ts[swapIndex] = ts[i];
            ts[i] = swap; 
        }
        
        return ts; 
    }
}
