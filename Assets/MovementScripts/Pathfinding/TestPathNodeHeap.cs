using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathNodeHeap : MonoBehaviour
{
    public void Start()
    {
        PathNodeHeap heap = new PathNodeHeap();

        for (int i = 1000; i > 0; i--)
        {
            PathNode insert = new PathNode(new Vector2Int(), null);
            insert.f = Random.Range(0.01f, 1000f);
            heap.Insert(insert);
        }

        PathNode prev = heap.DeleteMin();
        
        int removes = 1;
        while (!heap.IsEmpty())
        {
            PathNode nextMin = heap.DeleteMin();
            removes++;

            if (nextMin.f < prev.f)
            {
                throw new System.Exception("Heap returned out of order!");
            }
            prev = nextMin;
        }

        if (removes == 1000)
        {
            print("test passed!");
        } else
        {
            throw new System.Exception("removed " + removes + " items!");
        }
    }
}
