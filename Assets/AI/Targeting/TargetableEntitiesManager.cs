using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetableEntitiesManager : MonoBehaviour
{
    private static List<ILivingEntity> AllTargetables = new List<ILivingEntity>();
    
    public static void AddTargetable(ILivingEntity targetable)
    {
        AllTargetables.Add(targetable);
    }

    public static void RemoveTargetable(ILivingEntity targetable)
    {
        AllTargetables.Remove(targetable);
    }

    public static List<ILivingEntity> GetTargetables()
    {
        return AllTargetables;
    }
}
