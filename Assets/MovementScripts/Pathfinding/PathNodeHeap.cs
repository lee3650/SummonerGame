using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNodeHeap 
{
    private List<SearchNode> Data;
    private int LastItem;
    private Dictionary<Vector2Int, List<SearchNode>> Table;

    public PathNodeHeap()
    {
        Data = new List<SearchNode>();
        Table = new Dictionary<Vector2Int, List<SearchNode>>();
        LastItem = -1;
    }

    public bool IsEmpty()
    {
        return LastItem == -1;
    }

    public SearchNode PeekMin()
    {
        if (IsEmpty())
        {
            return null; 
        }

        return Data[0];
    }

    public void Insert(SearchNode node)
    {
        LastItem++;

        if (LastItem == Data.Count) //minor issue with this - we're never decreasing the size. 
        {
            Data.Add(node);
        } else
        {
            Data[LastItem] = node;
        }

        if (Table.TryGetValue(node.Pos, out List<SearchNode> nodes))
        {
            nodes.Add(node);
        } else
        {
            Table[node.Pos] = new List<SearchNode>() { node };
        }

        SwimUp(LastItem);
    }

    private void SwimUp(int index)
    {
        if (index == 0)
        {
            return;
        }

        SearchNode par = Data[GetParent(index)];

        if (Data[index].f < par.f)
        {
            Swap(index, GetParent(index));
            SwimUp(GetParent(index));
        }
    }

    public List<SearchNode> NodesAtPoint(Vector2Int point)
    {
        if (Table.TryGetValue(point, out List<SearchNode> result))
        {
            return result;
        }
        return new List<SearchNode>();
    }

    public SearchNode DeleteMin()
    {
        SearchNode result = Data[0]; //we should throw an exception if the heap is empty, so. 

        Data[0] = Data[LastItem];
        Data[LastItem] = null;
        LastItem--;

        if (!IsEmpty())
        {
            SinkDown(0);
        }

        Table[result.Pos].Remove(result); //hm... hopefully that works lol 

        return result; 
    }

    private void SinkDown(int index)
    {
        int l = 2 * index + 1;
        int r = 2 * index + 2;

        if (l <= LastItem)
        {
            if (r <= LastItem)
            {
                //both a left and right child 
                SearchNode lc = Data[l];
                SearchNode rc = Data[r];

                int min = lc.f < rc.f ? l : r; //if lc is < rc, then the minimum is at l. Otherwise, it's at r.

                if (Data[index].f > Data[min].f)
                {
                    Swap(index, min);
                    SinkDown(min);
                }
            } 
            else
            {
                //only a left child 
                SearchNode lc = Data[l];

                if (Data[index].f > lc.f)
                {
                    Swap(index, l);
                    SinkDown(l);
                }

            }
        } 
        //otherwise, we're done
        //if we don't have a left child we can't have a right child 
    }

    private int GetParent(int index)
    {
        return ((index - 1) / 2);
    }

    private void Swap(int a, int b)
    {
        SearchNode temp = Data[a];

        Data[a] = Data[b];
        Data[b] = temp;
    }
}
