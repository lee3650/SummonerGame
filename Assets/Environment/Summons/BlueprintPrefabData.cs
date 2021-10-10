using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BlueprintPrefabData
{
    public BlueprintType BlueprintType;
    public GameObject Prefab;
    public float MaintenanceFee;
    public string DisplayName; 

    public override string ToString()
    {
        return string.Format("{0}", DisplayName);
    }
}
