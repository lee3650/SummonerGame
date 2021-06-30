using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    [SerializeField] Camera FixedCamera;
    [SerializeField] Transform Player; 

    public void FixedUpdate()
    {
        Vector2 mousePos = GetWorldMousePos();
        transform.position = ((Vector2)Player.position + mousePos) / 2;
    }

    Vector2 GetWorldMousePos()
    {
        return FixedCamera.ScreenToWorldPoint(Input.mousePosition);
    }
}
