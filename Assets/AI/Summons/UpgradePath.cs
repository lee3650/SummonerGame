using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePath : MonoBehaviour
{
    [SerializeField] GameObject NextSummon;
    [SerializeField] float UpgradeCost;

    [SerializeField] private bool useable = false;

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
        ControllableSummon s;
        if (NextSummon.TryGetComponent<ControllableSummon>(out s))
        {
            return s.GetStatString() + "\nUpgrade Cost: " + UpgradeCost;
        }
        return "";
    }

    public bool Useable
    {
        get
        {
            return useable; 
        } 
        set
        {
            useable = value; 
        }
    }
}
