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
    public float CalculatedMinimum;
    public float CalculatedMaximum;

    public GameplayChange RequiredChange = GameplayChange.None; 

    [Tooltip("Used by letters")]
    public int startingLevel;
}
