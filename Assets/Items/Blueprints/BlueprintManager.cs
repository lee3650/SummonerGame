using System;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintManager : MonoBehaviour, IResettable
{
    public static event Action<BlueprintType> BlueprintAdded = delegate { };
    public static event Action<BlueprintType> BlueprintRemoved = delegate { }; //really this should tell you what was added, at the very least
    public static event Action BlueprintsChanged = delegate { }; //really this should tell you what was added, at the very least
    private static List<Blueprint> Blueprints = new List<Blueprint>();
    private static Dictionary<Vector2Int, Blueprint> BlueprintPositions = new Dictionary<Vector2Int, Blueprint>();
    
    public static void AddBlueprint(Vector2Int point, BlueprintType type, float rotation, float fee)
    {
        Blueprint add = new Blueprint(point, type, rotation, fee);

        BlueprintPositions[point] = add; 
        Blueprints.Add(add);

        BlueprintAdded(type);
        BlueprintsChanged();
    }

    public static void SetFeesForType(BlueprintType type, float start, float delta)
    {
        int cur = 0;

        for (int i = 0; i < Blueprints.Count; i++)
        {
            Blueprint b = Blueprints[i];
            if (b.BlueprintType == type)
            {
                b.MaintenanceFee = start + (cur * delta);
                print("Type " + type.ToString() + ", new maintenance fee: " + b.MaintenanceFee);
                cur++;
            }
        }
    }

    public static List<Blueprint> GetSatisfiedBlueprints()
    {
        List<Blueprint> result = new List<Blueprint>();

        foreach (Blueprint b in Blueprints)
        {
            if (b.Satisfied)
            {
                result.Add(b);
            }
        }

        return result;
    }

    public static List<Blueprint> GetSatisfiedBlueprints(BlueprintType type)
    {
        List<Blueprint> result = new List<Blueprint>();

        foreach (Blueprint b in Blueprints)
        {
            if (b.Satisfied && b.BlueprintType == type)
            {
                result.Add(b);
            }
        }

        return result;
    }

    //this isn't great practice - I've got a ton of duplication here, actually. 
    public static string GetNameFromBlueprintType(BlueprintType bt)
    {
        switch (bt)
        {
            case BlueprintType.Archer:
                return "Archer Unit";
            case BlueprintType.ArrowTrap:
                return "Ballista";
            case BlueprintType.Melee:
                return "Melee Unit";
            case BlueprintType.ArrowTurner:
                return "Arrow Turner";
            case BlueprintType.Gate:
            case BlueprintType.SpikeTrap:
            case BlueprintType.Wall:
            default:
                return bt.ToString();
        }
    }

    public static bool IsPointTaken(Vector2Int point)
    {
        Blueprint b;
        BlueprintPositions.TryGetValue(point, out b);
        if (b != null)
        {
            return true; 
        }
        return false; 
    }

    public static void SetSatisfied(Vector2Int point, bool Satisfied)
    {
        //technically these are all references to the same objects, so it should also update the list blueprints. 
        Blueprint set;
        if (BlueprintPositions.TryGetValue(point, out set))
        {
            set.Satisfied = Satisfied;
        }
    }

    public static float GetMaxSatisfiedFee(BlueprintType type)
    {
        List<Blueprint> bs = GetSatisfiedBlueprints(type);

        float max = -1f;

        foreach (Blueprint b in bs)
        {
            if (b.MaintenanceFee > max)
            {
                max = b.MaintenanceFee;
            }
        }

        return max;
    }

    public static bool ShouldRemoveSummon(Vector2Int point, BlueprintType type)
    {
        Blueprint print;
        BlueprintPositions.TryGetValue(point, out print);    
        if (print == null)
        {
            return true; 
        }
        if (print.BlueprintType != type)
        {
            return true; 
        }
        return false; 
    }

    public static Blueprint TryRemoveBlueprint(Vector2Int point)
    {
        if (BlueprintPositions.ContainsKey(point))
        {
            BlueprintPositions.Remove(point);
        }

        for (int i = Blueprints.Count - 1; i >= 0; i--)
        {
            if (Blueprints[i].Point == point)
            {
                Blueprint r = Blueprints[i];
                Blueprints.RemoveAt(i);
                BlueprintRemoved(r.BlueprintType);
                BlueprintsChanged();
                return r;
            }
        }

        return null; 
    }

    public static void ForceBlueprintsChanged()
    {
        BlueprintsChanged();
    }

    public static List<Blueprint> GetAdjacentBlueprints(Vector2Int point) 
    {
        Vector2Int[] dirs = new Vector2Int[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(0, -1),
        };

        List<Blueprint> prints = new List<Blueprint>();

        foreach (Vector2Int d in dirs)
        {
            Vector2Int newPoint = point + d;
            Blueprint b;
            BlueprintPositions.TryGetValue(newPoint, out b);
            if (b != null)
            {
                prints.Add(b);
            }
        }

        return prints; 
    }

    public static List<Blueprint> GetBlueprintsOfTypes(List<BlueprintType> types, bool onlySatisfied)
    {
        List<Blueprint> result = new List<Blueprint>();

        foreach (Blueprint b in Blueprints)
        {
            if (types.Contains(b.BlueprintType) && (!b.Satisfied || !onlySatisfied))
            {
                result.Add(b);
            }
        }

        return result; 
    }

    public void ResetState()
    {
        BlueprintsChanged = null;
        BlueprintsChanged = delegate { };
        BlueprintAdded = null;
        BlueprintAdded = delegate { };

        BlueprintRemoved = null;
        BlueprintRemoved = delegate { };

        Blueprints = new List<Blueprint>();
        BlueprintPositions = new Dictionary<Vector2Int, Blueprint>();
    }
}
