using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathfinding : MonoBehaviour
{
    [SerializeField] WaveSpawner WaveSpawner;

    public void RunTest()
    {
        PathManager.DrawPath(VectorRounder.RoundVectorToInt(WaveSpawner.GetSpawnPoints()[0]));
    }
}
