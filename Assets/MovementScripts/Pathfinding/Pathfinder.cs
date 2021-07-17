using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    //so, we're going to want to pass the end in as the start, in general, because that way it'll return our position as the first node, and we can just follow 'next' till we get to our target. 
    public static SearchNode GetPathFromPointToPoint(Vector2 start, Vector2 end)
    {
        start = RoundVectorToInt(start);
        end = RoundVectorToInt(end);

        if (IsStartAndEndInvalid(start, end))
        {
            print("The start or end was not valid!");
            return null; 
        }

        SearchNode openHead = GetStartNode(start);
        
        Dictionary<string, SearchNode> closedList = new Dictionary<string, SearchNode>(); //this way we get random access with the closed list. 
        //the downside is we have to construct a bunch of strings, so. 
        
        int counter = 0; 
        
        while (openHead != null && counter < 500)
        {
            SearchNode q = openHead;

            openHead = openHead.NextNode;

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (ShouldProcessPoint(q, x, y))
                    {
                        SearchNode successor = new SearchNode(q.x + x, q.y + y, q);

                        if (IsNodeGoal(successor, end))
                        {
                            return successor;
                        }

                        SetNodeGHF(successor, end);

                        if (ShouldInsertNodeIntoOpenList(successor, openHead, closedList))
                        {
                            openHead = InsertNodeIntoList(openHead, successor);
                        }
                    }
                }
            }

            counter++;
            InsertItemInClosedListIfLowerFValue(closedList, q);
        }

        print("Could not find path to target!");
        return null; 
    }

    static bool IsStartAndEndInvalid(Vector2 start, Vector2 end)
    {
        return MapManager.IsPointTraversable(start) == false || MapManager.IsPointTraversable(end) == false;
    }

    static SearchNode GetStartNode(Vector2 start)
    {
        SearchNode node = new SearchNode((int)start.x, (int)start.y);
        node.f = 0f;
        node.g = 0f;
        node.h = 0f;
        return node; 
    }

    static void SetNodeGHF(SearchNode successor, Vector2 end)
    {
        successor.g = successor.ParentNode.g + MapManager.GetTraversalCost(successor.x, successor.y);
        successor.h = Mathf.Abs(end.x - successor.x) + Mathf.Abs(end.y - successor.y);
        successor.f = successor.g + successor.h;
    }

    static bool ShouldInsertNodeIntoOpenList(SearchNode successor, SearchNode openHead, Dictionary<string, SearchNode> closedList)
    {
        return DoesNodeHaveLowerFValueThanInOpenList(openHead, successor) && DoesNodeHaveLowerFValueThanInClosedList(closedList, successor);
    }

    static bool IsNodeGoal(SearchNode node, Vector2 goal)
    {
        return node.x == (int)goal.x && node.y == (int)goal.y;
    }

    static bool ShouldProcessPoint(SearchNode q, int x, int y)
    {
        return (x != 0 || y != 0) && CanGoFromPointToPoint(q.x, q.y, q.x + x, q.y + y);
    }

    public static Vector2 RoundVectorToInt(Vector2 vector)
    {
        return new Vector2(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
    }

    static void InsertItemInClosedListIfLowerFValue(Dictionary<string, SearchNode> closedList, SearchNode node)
    {
        SearchNode item;
        if (closedList.TryGetValue(GetNodeKey(node), out item))
        {
            if (item.f > node.f)
            {
                closedList[GetNodeKey(node)] = node; 
            }
        } else
        {
            closedList[GetNodeKey(node)] = node;
        }
    }

    static string GetNodeKey(SearchNode node)
    {
        return string.Format("{0},{1}", node.x, node.y);
    }

    static bool DoesNodeHaveLowerFValueThanInOpenList(SearchNode openHead, SearchNode node)
    {
        SearchNode lowestFNode = FindLowestFNodeInList(openHead, node);

        if (lowestFNode == null)
        {
            return true; 
        }
        return node.f < lowestFNode.f;
    }

    static bool DoesNodeHaveLowerFValueThanInClosedList(Dictionary<string, SearchNode> closedList, SearchNode node)
    {
        SearchNode find;
        if (closedList.TryGetValue(GetNodeKey(node), out find))
        {
            if (node.f < find.f)
            {
                return true; 
            } else
            {
                return false; 
            }
        }
        return true; 
    }

    static bool CanGoFromPointToPoint(int startX, int startY, int endX, int endY)
    {
        if (!MapManager.IsPointTraversable(endX, endY))
        {
            return false; 
        }

        if (startX - endX != 0 && startY - endY != 0)
        {
            //so, a diagonal move. 
            return MapManager.IsPointTraversable(startX, endY) && MapManager.IsPointTraversable(endX, startY);
        }

        return true; //so, we know the point is traversable already, so we're done. 
    }

    static SearchNode FindLowestFNodeInList(SearchNode head, SearchNode node)
    {
        SearchNode cur = head;

        SearchNode result = null; 

        while (cur != null)
        {
            if (cur.x == node.x && cur.y == node.y)
            {
                if (result == null || cur.f < result.f)
                {
                    result = cur; 
                }
            }

            cur = cur.NextNode; 
        }

        return result; 
    }

    static SearchNode InsertNodeIntoList(SearchNode head, SearchNode node)
    {
        if (head == null)
        {
            return node; 
        }

        if (node.f < head.f)
        {
            node.NextNode = head;
            return node; 
        }
        
        SearchNode cur = head; 
        
        while (cur != null)
        {
            if (node.f >= cur.f && (cur.NextNode == null || node.f < cur.NextNode.f))
            {
                node.NextNode = cur.NextNode;
                cur.NextNode = node;
                return head; 
            }    

            cur = cur.NextNode; 
        }

        throw new Exception("Somehow our insertion failed!"); 
    }
}
