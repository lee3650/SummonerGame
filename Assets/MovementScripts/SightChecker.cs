using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightChecker : MonoBehaviour
{
    public bool NoUnbreakableWallsInWay(Vector2 goal)
    {
        return NoCollidersInPathHaveTag("Unbreakable", goal);
    }

    //welp, just made that less efficient 
    public bool CanSeePathToTarget(Vector2 goal)
    {
        return NoCollidersInPathHaveTags(new List<string>() {"Unbreakable", "Untraversable"}, goal);
    }

    private Vector2 CalculateNormal(Vector2 goal)
    {
        Vector2 normal = (goal - (Vector2)transform.position).normalized;
        normal = new Vector2(-normal.y, normal.x);
        return normal;
    }

    private RaycastHit2D[] GetUpperHits(Vector2 goal)
    {
        Vector2 normal = CalculateNormal(goal);
        RaycastHit2D[] upperHits = Physics2D.RaycastAll(((Vector2)transform.position + (0.4f * normal)), (goal - (Vector2)transform.position), Vector2.Distance(goal, transform.position));
        return upperHits;
    }

    private RaycastHit2D[] GetLowerHits(Vector2 goal)
    {
        Vector2 normal = CalculateNormal(goal);
        RaycastHit2D[] lowerHits = Physics2D.RaycastAll((Vector2)transform.position + (-0.4f * normal), (goal - (Vector2)transform.position), Vector2.Distance(goal, transform.position));
        return lowerHits;
    }

    //multiple tags

    private bool NoCollidersInPathHaveTags(List<string> tags, Vector2 goal)
    {
        RaycastHit2D[] lowerHits = GetLowerHits(goal);
        RaycastHit2D[] upperHits = GetUpperHits(goal);

        if (NoRaycastHitTags(lowerHits, tags) && NoRaycastHitTags(upperHits, tags))
        {
            return true;
        }

        return false;
    }

    bool NoRaycastHitTags(RaycastHit2D[] hits, List<string> tags)
    {
        foreach (RaycastHit2D hit in hits)
        {
            if (tags.Contains(hit.transform.tag))
            {
                return false;
            }
        }
        return true;
    }

    //single tag

    private bool NoCollidersInPathHaveTag(string tag, Vector2 goal)
    {
        RaycastHit2D[] lowerHits = GetLowerHits(goal);
        RaycastHit2D[] upperHits = GetUpperHits(goal);

        if (NoRaycastHitTag(lowerHits, tag) && NoRaycastHitTag(upperHits, tag))
        {
            return true; 
        }

        return false; 
    }

    bool NoRaycastHitTag(RaycastHit2D[] hits, string tag)
    {
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.CompareTag(tag))
            {
                return false;
            }
        }
        return true; 
    }
}
