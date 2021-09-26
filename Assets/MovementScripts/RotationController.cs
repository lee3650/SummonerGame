using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float RotationSpeed;
    [SerializeField] DirectionToSprite[] directionToSprites = new DirectionToSprite[4];
    [SerializeField] SpriteRenderer sr;
    [SerializeField] bool RotateByChangingSprite; 

    void Start()
    {

    }

    public void FacePoint(Vector2 worldPoint)
    {
        float z = GetDirectionFacingPoint(worldPoint);
        rb.rotation = Mathf.LerpAngle(rb.rotation, z, RotationSpeed * Time.deltaTime);
    }

    public void StopRotation()
    {
        rb.angularVelocity = 0f;
    }

    public void FaceForward()
    {
        FaceDirection(rb.velocity.normalized);
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
        /*
        if (RotateByChangingSprite)
        {
            rb.rotation = 0f;
            Vector2Int rounded = VectorRounder.RoundVectorToInt(dir);
            if (Mathf.Abs(rounded.x) > 0 && Mathf.Abs(rounded.y) > 0)
            {
                rounded = new Vector2Int(0, rounded.y);
            }
            if (rounded != Vector2Int.zero)
            {
                sr.sprite = GetSprite(rounded);
            }
        } else
        {
            float z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            z -= 90f;
            rb.rotation = z;
        }
         */ 
         //basically this method and this script are obsolete.
    }

    private Sprite GetSprite(Vector2Int dir)
    {
        foreach (DirectionToSprite ds in directionToSprites)
        {
            if (ds.Direction == dir)
            {
                return ds.Sprite;
            }
        }
        throw new Exception("Could not find sprite for direction: " + dir);
    }
}
