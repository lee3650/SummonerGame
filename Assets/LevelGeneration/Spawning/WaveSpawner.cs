using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> Enemies;


    [SerializeField] Transform BottomLeft, TopRight;
    [SerializeField] bool UseTransforms; 

    List<Vector2> SpawnRegion;

    List<AIEntity> CurrentWave = new List<AIEntity>();

    private void Awake()
    {
        if (UseTransforms)
        {
            SpawnRegion = LevelGenerator.GetPointsWithinBoundaries(BottomLeft.position, TopRight.position);
        }
    }

    public void SetSpawnRegion(List<Vector2> region)
    {
        SpawnRegion = region; 
    }

    public void SpawnWave(List<GameObject> wave)
    {
        CurrentWave = new List<AIEntity>();

        for (int i = 0; i < wave.Count; i++)
        {
            GameObject enemy = Instantiate(wave[i], GetRandomPointInSpawnZone(), Quaternion.Euler(Vector3.zero));
            enemy.GetComponent<AIEntity>().WakeUp();
            CurrentWave.Add(enemy.GetComponent<AIEntity>());
        }
    }

    public bool CurrentWaveDefeated()
    {
        foreach (AIEntity aIEntity in CurrentWave)
        {
            if (aIEntity.IsAlive())
            {
                return false; 
            }
        }

        return true; 
    }

    Vector2 GetRandomPointInSpawnZone()
    {
        return SpawnRegion[Random.Range(0, SpawnRegion.Count)]; 
    }
}
