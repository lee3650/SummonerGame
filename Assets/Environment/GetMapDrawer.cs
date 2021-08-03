using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMapDrawer : MonoBehaviour
{
    [SerializeField] MapDrawer MapDrawer;
    private static MapDrawer MapDrawerStatic;

    void Awake()
    {
        MapDrawerStatic = MapDrawer;
    }

    public static MapDrawer GetSceneMapDrawer()
    {
        return MapDrawerStatic;
    }
}
