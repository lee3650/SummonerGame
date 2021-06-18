using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float RotationSpeed;

    void Start()
    {

    }

    public void FacePoint(Vector2 worldPoint)
    {
        float z = GetDirectionFacingPoint(worldPoint);
        rb.rotation = Mathf.LerpAngle(rb.rotation, z, RotationSpeed * Time.deltaTime);
    }

    public float GetDirectionFacingPoint(Vector2 worldPoint)
    {
        float deltaX = worldPoint.x - rb.position.x;
        float deltaY = worldPoint.y - rb.position.y;

        float z = Mathf.Atan2(deltaY, deltaX) * Mathf.Rad2Deg;

        z -= 90f; //this is still necessary because (0, 1) is considered an angle of 0 degrees, but it should be -90. 

        return z;
    }

    public float GetCurrentRotation()
    {
        return rb.rotation;
    }

    public void FaceDirection(Vector2 dir)
    {
        float z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        z -= 90f;

        rb.rotation = z;
    }
}
