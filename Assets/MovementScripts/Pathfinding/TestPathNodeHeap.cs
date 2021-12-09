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
            SearchNode insert = new SearchNode(new Vector2Int(), null);
            insert.f = Random.Range(0.01f, 1000f);
            heap.Insert(insert);
        }

        SearchNode prev = heap.DeleteMin();
        
        int removes = 1;
        while (!heap.IsEmpty())
        {
            SearchNode nextMin = heap.DeleteMin();
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
