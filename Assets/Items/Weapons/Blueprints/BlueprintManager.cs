using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintManager : MonoBehaviour
{
    public static event Action BlueprintsChanged = delegate { };
    static List<Blueprint> Blueprints = new List<Blueprint>();

    public static void AddBlueprint(Vector2 point, BlueprintType type)
    {
        for (int i = Blueprints.Count - 1; i >= 0; i--)
        {
            if (Blueprints[i].Point == point)
            {
                Blueprints[i] = new Blueprint(point, type);
                BlueprintsChanged();
                return; 
            }
        }

        Blueprints.Add(new Blueprint(point, type));

        BlueprintsChanged();
    }

    public static void SetSatisfied(Vector2 point, bool Satisfied)
    {
        foreach (Blueprint b in Blueprints)
        {
            if (b.Point == point)
            {
                b.Satisfied = Satisfied;
                return; 
            }
        }
    }

    public static void RemoveBlueprint(Vector2 point)
    {
        for (int i = Blueprints.Count - 1; i >= 0; i--)
        {
            if (Blueprints[i].Point == point)
            {
                Blueprints.RemoveAt(i);
                BlueprintsChanged();
                return;
            }
        }
    }

    public static List<Blueprint> GetBlueprintsOfTypes(List<BlueprintType> types)
    {
        List<Blueprint> result = new List<Blueprint>();

        foreach (Blueprint b in Blueprints)
        {
            if (types.Contains(b.BlueprintType))
            {
                result.Add(b);
            }
        }

        return result; 
    }
}
