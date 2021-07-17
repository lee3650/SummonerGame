using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] Camera Camera;
    [SerializeField] float Sensitivity; 
    void Update()
    {
        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            Camera.orthographicSize += Input.mouseScrollDelta.y * Sensitivity;
        }
    }
}
