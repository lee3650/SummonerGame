using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpawnToProbability
{
    public float RelativeLikelihood;
    public GameObject Spawn;
    public bool Enabled;
    [HideInInspector] public float CalculatedMinimum;
    [HideInInspector] public float CalculatedMaximum;
}
