using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeID : MonoBehaviour
{
    [SerializeField] SummonType SummonType;
    [SerializeField] int Identifier;

    public override bool Equals(object other)
    {
        UpgradeID otherUpgrade = other as UpgradeID;
        if (otherUpgrade == null)
        {
            return false; 
        }

        return otherUpgrade.SummonType == SummonType && otherUpgrade.Identifier == Identifier;
    }
}
