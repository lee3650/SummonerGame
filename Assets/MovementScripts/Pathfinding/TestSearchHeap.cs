using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSearchHeap : MonoBehaviour
{

    public void RunSearchHeapTests()
    {
        TestGetMin();
        TestRemovalGetMin();
        print("Tests passed!");
    }

    private void TestGetMin()
    {
        SearchNode[] vals = new SearchNode[]
        {
            new SearchNode(43),
            new SearchNode(9),
            new SearchNode(22),
            new SearchNode(178),
            new SearchNode(19),
            new SearchNode(90),
            new SearchNode(15),
            new SearchNode(40),
            new SearchNode(7),
            new SearchNode(4),
        };

        SearchNodeHeap heap = new SearchNodeHeap();

        for (int i = 0; i < vals.Length; i++)
        {
            heap.Insert(vals[i]);
        }

        SearchNode min = heap.GetMin();

        if (min.f != 4)
        {
            throw new System.Exception("Get min test failed!");
        }
    }

    private void TestRemovalGetMin()
    {
        SearchNode[] vals = new SearchNode[]
        {
            new SearchNode(43),
            new SearchNode(9),
            new SearchNode(22),
            new SearchNode(178),
            new SearchNode(19),
            new SearchNode(90),
            new SearchNode(15),
            new SearchNode(40),
            new SearchNode(7),
            new SearchNode(4),
        };

        SearchNodeHeap heap = new SearchNodeHeap();

        for (int i = 0; i < vals.Length; i++)
        {
            heap.Insert(vals[i]);
        }

        heap.printData();

        SearchNode min = heap.GetMin();

        print("first smallest: " + min.f);

        SearchNode next = heap.GetMin();
        heap.printData();

        if (next.f != 7)
        {
            throw new System.Exception("Removal get min test failed! Next smallest was: " + next.f);
        }

        while (heap.Size() > 1)
        {
            heap.GetMin();
        }
        SearchNode s = heap.GetMin();
        if (heap.IsEmpty() == false)
        {
            throw new System.Exception("Heap was not empty! Count: " + heap.Size());
        }

        if (s.f != 178)
        {
            throw new System.Exception("Last element was not largest!");
        }
    }

    private void TestRemovalThenAdd()
    {
        SearchNode[] vals = new SearchNode[]
        {
            new SearchNode(56),
            new SearchNode(31),
            new SearchNode(46),
            new SearchNode(55),
            new SearchNode(73),
            new SearchNode(26),
            new SearchNode(1),
            new SearchNode(89),
            new SearchNode(96),
            new SearchNode(9),
        };

        SearchNodeHeap heap = new SearchNodeHeap();

        for (int i = 0; i < vals.Length; i++)
        {
            heap.Insert(vals[i]);
        }

        for (int i = 0; i < vals.Length; i++)
        {
            heap.GetMin();
        }

        if (heap.IsEmpty() == false)
        {
            throw new System.Exception("Heap was not emptied properly!");
        }

        for (int i = 0; i < vals.Length; i++)
        {
            heap.Insert(vals[i]);
        }

        for (int i = 0; i < vals.Length; i++)
        {
            heap.Insert(vals[i]);
        }

        if (heap.GetMin().f != 1)
        {
            throw new System.Exception("Min was not correct with duplicates!");
        }

        if (heap.GetMin().f != 1)
        {
            throw new System.Exception("Min was not correct with duplicates the second time!");
        }

        if (heap.GetMin().f != 7)
        {
            throw new System.Exception("Min was not correct with duplicates the third time!");
        }
    }
}
