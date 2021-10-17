using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarshipManager : MonoBehaviour
{
    [SerializeField] WaveSpawner WaveSpawner;
    [SerializeField] int xSpawnDelta;
    [SerializeField] Warship WarshipPrefab;
    private List<Warship> SpawnedShips = new List<Warship>();
    
    int[] randomOffsets = new int[] 
    {
        0, 1, 2, 3
    };

    public void SpawnShips()
    {
        List<Vector2> spawnPoints = WaveSpawner.GetSpawnPoints();
        
        foreach (Vector2 v in spawnPoints)
        {
            if (PointIsNotCoveredByShips(v))
            {
                SpawnShip(v);
            }
        }
    }

    private void SpawnShip(Vector2 point)
    {
        Warship w = Instantiate<Warship>(WarshipPrefab, point + new Vector2(xSpawnDelta + randomOffsets[Random.Range(0, randomOffsets.Length)], 0), Quaternion.Euler(Vector3.zero));
        w.Endpoint = point;
        w.GoToEndPoint();
        SpawnedShips.Add(w);
    }

    private bool PointIsNotCoveredByShips(Vector2 point)
    {
        foreach (Warship w in SpawnedShips)
        {
            if (Vector2.Distance(w.Endpoint, point) <= 1.01f) 
            {
                return false;
            }
        }
        return true; 
    }

    public void SnapShips()
    {
        foreach (Warship w in SpawnedShips)
        {
            w.SnapToDestination();
        }
    }
}
