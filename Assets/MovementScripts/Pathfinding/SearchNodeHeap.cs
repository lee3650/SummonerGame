using System.Collections.Generic;
using UnityEngine;

public class SearchNodeHeap
{
    private List<SearchNode> Data;
    private int lastElement; 

    public SearchNodeHeap()
    {
        Data = new List<SearchNode>();
        lastElement = -1; 
    }

    public bool IsEmpty()
    {
        return Data.Count == 0;
    }

    public void printData()
    {
        foreach (SearchNode s in Data)
        {
            MonoBehaviour.print(s.f + ", ");
        }
    }

    public int Size()
    {
        return lastElement + 1;
    }

    public void Insert(SearchNode item)
    {
        Data.Add(item);
        lastElement++;
        SwimUp(lastElement);
    }

    private void SwimUp(int i)
    {
        if (i == 0)
        {
            return; 
        }
        
        int cur = i;
        SearchNode node = Data[cur]; //we actually never have to update 'node'
        SearchNode par = Data[GetParent(cur)];

        while (node.f < par.f && cur != 0)
        {
            Data[cur] = par;
            Data[GetParent(cur)] = node; 
            
            cur = GetParent(cur);

            if (cur != 0)
            {
                par = Data[GetParent(cur)];
            }
        }
    }

    public SearchNode GetMin()
    {
        //we're going to assume this is non-empty 
        SearchNode result = Data[0];

        Data[0] = Data[lastElement];
        Data.RemoveAt(lastElement);

        lastElement--;

        //now we swim down. First we check that data has elements. 
        if (lastElement > 0) //if it's equal to zero no use doing this - there's only one element in the array 
        {
            SwimDown();
        }

        return result; 
    }

    private void SwimDown()
    {
        //we don't even need an index because we know that Data[0] is the head. 
        SearchNode node = Data[0];
        int cur = 0;
        while (ShouldMoveDownward(cur, node)) 
        {
            int swapInd = GetLowestChild(cur);

            Data[cur] = Data[swapInd];
            Data[swapInd] = node;

            cur = swapInd;
        }
    }

    private int GetLowestChild(int i) //we have to assume of them exists, right. 
    {
        if (!RightChildExists(i))
        {
            return GetLeftChild(i); //um... it's left filling, so technically this is enough. 
        }
        if (!LeftChildExists(i)) //I'm afraid of that though, so I'm going to do this anyway
        {
            throw new System.Exception("Tree was not complete!");
        }

        if (Data[GetLeftChild(i)].f < Data[GetRightChild(i)].f)
        {
            return GetLeftChild(i);
        }

        return GetRightChild(i);
    }

    private bool AnyChildExists(int i)
    {
        return LeftChildExists(i) || RightChildExists(i);
    }

    private bool ShouldMoveDownward(int i, SearchNode n)
    {
        if (AnyChildExists(i))
        {
            if (LeftChildExists(i))
            {
                if (n.f > Data[GetLeftChild(i)].f)
                {
                    return true; 
                }
            } 
            if (RightChildExists(i))
            {
                if (n.f > Data[GetRightChild(i)].f)
                {
                    return true;
                }
            }
        }
        return false; 
    }

    private bool LeftChildExists(int i)
    {
        return GetLeftChild(i) <= lastElement;
    }

    private bool RightChildExists(int i)
    {
        return GetRightChild(i) <= lastElement;
    }

    public SearchNode Peek()
    {
        return Data[0];
    }

    private int GetParent(int i)
    {
        return (i - 1) / 2; //we technically don't need the floor, because it truncates. 
                            //I'm going to have to test this, aren't I. 
    }

    private int GetLeftChild(int i)
    {
        return 2 * i + 1;
    }
    
    private int GetRightChild(int i)
    {
        return 2 * i + 2;
    }
}
