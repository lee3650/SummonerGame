﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] MovementController MovementController;
    [SerializeField] RotationController RotationController;

    private ITargetable target;

    public bool HasTarget()
    {
        if (Target != null)
        {
            return true;
        }
        return false; 
    }

    public void MoveAtTarget()
    {
        Vector2 dir = Target.GetPosition() - (Vector2)transform.position;
        MovementController.MoveInDirection(dir.normalized);
    }

    public void LookAtTarget()
    {
        if (Target != null)
        {
            RotationController.FacePoint(Target.GetPosition());
        }
    }
    
    public float DistanceFromTargetToPoint(Vector2 point)
    {
        if (Target == null)
        {
            throw new System.Exception("Cannot give distance from point to null target!");
        }
        return Vector2.Distance(Target.GetPosition(), point); 
    }

    public bool IsTargetAlive()
    {
        if (HasTarget())
        {
            return Target.IsAlive();
        }
        return false;
    }

    public ITargetable Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
        }
    }
}
