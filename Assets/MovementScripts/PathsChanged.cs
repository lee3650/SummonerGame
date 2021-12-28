using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathsChanged : MonoBehaviour
{
    const float revealTime = 0.5f;
    float revealTimer = revealTime;

    private static bool pathChanged = false;
    private static bool revealFrame = false;

    private void Awake()
    {
        pathChanged = false;
        revealFrame = false;
        revealTimer = 1f;
    }

    public static bool RecalculatePathfinding()
    {
        if (revealFrame)
        {
            return pathChanged;
        }
        return false; 
    }

    public static void ResetPaths()
    {
        pathChanged = true; 
    }

    private void Update()
    {
        if (revealFrame)
        {
            revealFrame = false; 
        }

        revealTimer -= Time.deltaTime;
        if (revealTimer <= 0f)
        {
            revealTimer = revealTime;
            revealFrame = true;
        }
    }
}
