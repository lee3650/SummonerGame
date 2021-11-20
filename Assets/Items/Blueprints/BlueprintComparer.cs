using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintComparer : IComparer<Blueprint>
{
    public int Compare(Blueprint a, Blueprint b)
    {
        return a.MaintenanceFee.CompareTo(b.MaintenanceFee);
    }
}
