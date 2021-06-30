using System.Collections;
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
        RotationController.FacePoint(Target.GetPosition());
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
