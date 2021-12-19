using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class TargetableEntitiesManager : MonoBehaviour, IResettable
{
    private static List<ILivingEntity> AllTargetables = new List<ILivingEntity>();

    //hm. This is kind of messed up, isn't it. The position of a targetable isn't constant - not e v e n archers, because when they summon they're at a 
    //different point then when they, um, go to their point, right. 
    //hm. Well, with... okay. Well, honestly, for now I'll do brute force, but...
    //well, that's n^2... 
    //okay fine. I'll make an array for just friendlies. Fine. 
    public static void AddTargetable(ILivingEntity targetable)
    {
        AllTargetables.Add(targetable);
    }

    public static List<ILivingEntity> GetTargets(Factions faction, int exclusivePriority)
    {
        List<ILivingEntity> result = new List<ILivingEntity>();

        for (int i = 0; i < AllTargetables.Count; i++)
        {
            if (AllTargetables[i].GetFaction() == faction && AllTargetables[i].GetPrecedence() > exclusivePriority)
            {
                result.Add(AllTargetables[i]);
            }
        }

        return result; 
    }

    public static List<ILivingEntity> GetAllTargets(Vector2 pos, float rad)
    {
        List<ILivingEntity> result = new List<ILivingEntity>();

        for (int i = 0; i < AllTargetables.Count; i++)
        {
            if (Vector2.Distance(AllTargetables[i].GetPosition(), pos) < rad) //we can make this marginally faster by comparing with radius squared 
            {
                result.Add(AllTargetables[i]);
            }
        }

        return result; 
    }
 
    public static void RemoveTargetable(ILivingEntity targetable)
    {
        AllTargetables.Remove(targetable);
    }

    public static List<ILivingEntity> GetTargetables()
    {
        return AllTargetables;
    }

    public void ResetState()
    {
        AllTargetables = new List<ILivingEntity>();
    }
}
