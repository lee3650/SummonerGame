using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class TargetableEntitiesManager : MonoBehaviour, IResettable
{
    private static List<ILivingEntity> AllTargetables = new List<ILivingEntity>();
    private static ITargetable[,] SecondaryTargets; //so, let's do some lazy initialization, eh? 

    private static Vector2Int[] dirs = new Vector2Int[] {
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
    };

    //hm. This is kind of messed up, isn't it. The position of a targetable isn't constant - not e v e n archers, because when they summon they're at a 
    //different point then when they, um, go to their point, right. 
    //hm. Well, with... okay. Well, honestly, for now I'll do brute force, but...
    //well, that's n^2... 
    //okay fine. I'll make an array for just friendlies. Fine. 
    public static void AddTargetable(ILivingEntity targetable)
    {
        AllTargetables.Add(targetable);
    }

    public static void AddSecondaryTarget(Vector2Int position, ITargetable target)
    {
        if (SecondaryTargets == null)
        {
            SecondaryTargets = new ITargetable[MapManager.xSize, MapManager.ySize];
            //hm. When they die then they have to remove the secondary target as well. Okay. 

        }
        //this actually isn't that inefficient because it's only one array. 

        SecondaryTargets[position.x, position.y] = target;
    }

    public static void RemoveSecondaryTarget(Vector2Int point)
    {
        if (SecondaryTargets != null)
        {
            SecondaryTargets[point.x, point.y] = null;
        }
    }

    public static ITargetable GetTargetableAdjacentTo(Vector2Int point)
    {
        for (int i = 0; i < dirs.Length; i++)
        {
            Vector2Int t = new Vector2Int(point.x + dirs[i].x, point.y + dirs[i].y);
            if (t.x >= 0 && t.x < MapManager.xSize && t.y >= 0 && t.y < MapManager.ySize)
            {
                ITargetable target = SecondaryTargets[t.x, t.y];

                if (target != null)
                {
                    return target;
                }
            }
        }

        return null; 
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
        SecondaryTargets = null;
    }
}
