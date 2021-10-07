using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToWorldPoint : MonoBehaviour
{
    [SerializeField] Vector2 WorldPoint;
    [SerializeField] Camera MainCamera;

    public void SetWorldPoint(Vector2 point)
    {
        WorldPoint = point;
        transform.position = (Vector2)MainCamera.WorldToScreenPoint(WorldPoint);
    }

    private void FixedUpdate()
    {
        transform.position = (Vector2)MainCamera.WorldToScreenPoint(WorldPoint);
    }
}
