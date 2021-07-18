using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightChecker : MonoBehaviour
{
    public bool CanSeePathToTarget(Vector2 goal)
    {
        Vector2 normal = (goal - (Vector2)transform.position).normalized;
        normal = new Vector2(-normal.y, normal.x);

        RaycastHit2D[] upperHits = Physics2D.RaycastAll(((Vector2)transform.position + (0.4f * normal)), (goal - (Vector2)transform.position), Vector2.Distance(goal, transform.position));
        RaycastHit2D[] lowerHits = Physics2D.RaycastAll((Vector2)transform.position + (-0.4f * normal), (goal - (Vector2)transform.position), Vector2.Distance(goal, transform.position));

        foreach (RaycastHit2D hit in upperHits)
        {
            if (hit.transform.CompareTag("Untraversable"))
            {
                return false;
            }
        }
        foreach (RaycastHit2D hit in lowerHits)
        {
            if (hit.transform.CompareTag("Untraversable"))
            {
                return false;
            }
        }

        return true;
    }
}
