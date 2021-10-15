using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCenterManager : MonoBehaviour
{
    public static Vector2 MapCenter
    {
        get
        {
            return mapCenter;
        }
    }

    private static Vector2 mapCenter;
    
    private void Awake()
    {
        mapCenter = transform.position;
    }
}
