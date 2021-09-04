using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintSummonSpawnIndicator : MonoBehaviour
{
    [SerializeField] GameObject indicator;
    [SerializeField] BlueprintBarracks BlueprintBarracks;

    private void Awake()
    {
        GameObject i = Instantiate(indicator, Vector3Int.zero, Quaternion.Euler(Vector3.zero), transform);
        i.transform.localPosition = (Vector3Int)BlueprintBarracks.GetSpawnOffset();
    }
}
