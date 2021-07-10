using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float MoveSpeed;
    [SerializeField] float Sensitivity;

    Vector2 pathfindGoal;
    SearchNode pathfindPath = null; 

    void Start()
    {
        
    }

    public void MoveTowardPoint(Vector2 worldPoint)
    {
        Vector2 dir = worldPoint - rb.position;
        MoveInDirection(dir.normalized);
    }

    public void MonitorGoalAndFollowPath()
    {
        if (pathfindPath == null)
        {
            //print("pathfindpath was null!");
            return; 
        }

        MoveTowardPoint(new Vector2(pathfindPath.x, pathfindPath.y));

        if (Vector2.Distance(new Vector2(pathfindPath.x, pathfindPath.y), transform.position) < 0.1f)
        {
            pathfindPath = pathfindPath.ParentNode; 
        }
    }

    public void SetPathfindGoal(Vector2 goal)
    {
        pathfindGoal = goal;
        pathfindPath = Pathfinder.GetPathFromPointToPoint(goal, transform.position);
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

    public void DisableRigidbody()
    {
        rb.isKinematic = true;
    }

    public void DisableAllMovement()
    {
        rb.bodyType = RigidbodyType2D.Static;
    }

    public void SetVelocity(Vector2 direction, float magnitude)
    {
        rb.velocity = direction * magnitude;
    }

}
