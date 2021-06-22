using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    private ILivingEntity target;

    public bool HasTarget()
    {
        if (Target != null)
        {
            return true;
        }
        return false; 
    }

    public ILivingEntity Target
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
