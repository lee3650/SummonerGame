using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePath : MonoBehaviour
{
    [SerializeField] GameObject NextSummon;
    [SerializeField] float UpgradeCost;
    [SerializeField] SummonType summonType;
    [SerializeField] int tier;

    public GameObject GetNextSummon()
    {
        return NextSummon;
    }
    public float GetUpgradeCost()
    {
        return UpgradeCost;
    }

    public string GetNextSummonStats()
    {
        IControllableSummon s;
        if (NextSummon.TryGetComponent<IControllableSummon>(out s))
        {
            return s.GetStatString() + "\nUpgrade Cost: " + UpgradeCost;
        }
        return "";
    }

    public int Tier
    {
        get
        {
            return tier;
        }
    }

    public SummonType SummonType
    {
        get
        {
            return summonType;
        }
    }
}
