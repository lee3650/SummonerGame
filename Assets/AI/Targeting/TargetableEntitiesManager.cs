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
    //do we need to remove them? Eh, probably not. 

    public static List<ILivingEntity> GetTargetables()
    {
        return AllTargetables;
    }
}
