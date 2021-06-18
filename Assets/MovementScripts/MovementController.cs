using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float MoveSpeed;
    [SerializeField] float Sensitivity;

    void Start()
    {
        
    }

    public void MoveInDirection(Vector2 dir)
    {
        float mag = dir.magnitude;
        
        float speed = MoveSpeed;
        
        if (mag >= 1)
        {
            speed = MoveSpeed / mag;
        }

        Vector2 targetVel = dir * speed;

        rb.velocity = Vector2.Lerp(rb.velocity, targetVel, Sensitivity * Time.deltaTime);
    }

    public void SetVelocity(Vector2 direction, float magnitude)
    {
        rb.velocity = direction * magnitude;
    }

}
