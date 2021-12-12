using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//This is actually pretty clean other than the observer pattern 

public class WaveSpawner : MonoBehaviour, IResettable
{
    [SerializeField] List<GameObject> Enemies;

    [SerializeField] Transform BottomLeft, TopRight;
    [SerializeField] bool UseTransforms;

    static List<IWaveNotifier> ClientsToNotify = new List<IWaveNotifier>();

    List<Vector2> SpawnRegion = new List<Vector2>();
    List<Vector2> AvailablePoints = new List<Vector2>();
    List<AIEntity> CurrentWave = new List<AIEntity>();

    bool NotifiedWaveCompletion = true;

    public static bool IsCurrentWaveDefeated = true; 

    public void ResetState()
    {
        ClientsToNotify = new List<IWaveNotifier>();
        IsCurrentWaveDefeated = true; 
    }

    private void Awake()
    {
        if (UseTransforms)
        {
            SpawnRegion = LevelGenerator.GetPointsWithinBoundaries(BottomLeft.position, TopRight.position);
        }
    }

    public List<Vector2> GetSpawnPoints()
    {
        return AvailablePoints;
    }

    public void MakeRandomPointsAvailable()
    {
        int points = Random.Range(1, 2); 
        for (int i = 0; i < points; i++)
        {
            if (SpawnRegion.Count == 0)
            {
                break;
            }

            int ind = Random.Range(0, SpawnRegion.Count);
            AvailablePoints.Add(SpawnRegion[ind]);
            SpawnRegion.RemoveAt(ind);
        }

        foreach (Vector2 v in AvailablePoints)
        {
            print("Available spawn point: " + v);
        }
    }

    public bool IsPointInSpawnRegion(Vector2 point)
    {
        return SpawnRegion.Contains(point) || AvailablePoints.Contains(point);
    }

    public void AddSpawnRegion(List<Vector2> region)
    {
        SpawnRegion.AddRange(region);
    }

    public static void NotifyWhenWaveEnds(IWaveNotifier notifier)
    {
        ClientsToNotify.Add(notifier); //we're looping backwards, so, the first thing added is called last. 
    }

    public static void StopNotifyingWhenWaveEnds(IWaveNotifier notifier)
    {
        ClientsToNotify.Remove(notifier);
    }

    public void SpawnWave(List<GameObject> wave, float spawnTime)
    {
        CurrentWave = new List<AIEntity>();

        for (int i = 0; i < wave.Count; i++)
        {
            GameObject enemy = Instantiate(wave[i], GetRandomPointInSpawnZone(), Quaternion.Euler(Vector3.zero));
            //enemy.GetComponent<AIEntity>().WakeUp();
            CurrentWave.Add(enemy.GetComponent<AIEntity>());
        }

        StartCoroutine(EnableEnemies(spawnTime));

        print("Spawned entities!");

        NotifiedWaveCompletion = false;
        IsCurrentWaveDefeated = false; 
    }

    private IEnumerator EnableEnemies(float spawnTime)
    {
        int curIndex = 0;

        bool loop = true; 

        while (loop)
        {
            for (int i = curIndex; i < curIndex + 1; i++)
            {
                if (i < CurrentWave.Count)
                {
                    CurrentWave[i].WakeUp();
                } else
                {
                    loop = false;
                }
            }
            
            curIndex += 1; 

            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void ResetSpawnRegion()
    {
        SpawnRegion = new List<Vector2>();
    }

    private void LateUpdate() //Why is this in late update? 
    {
        if (CurrentWaveDefeated() && !NotifiedWaveCompletion)
        {
            print("Notifying clients!");

            for (int i = ClientsToNotify.Count - 1; i >= 0; i--)
            {
                ClientsToNotify[i].OnWaveEnds();
            }

            IsCurrentWaveDefeated = true; 
            NotifiedWaveCompletion = true; 
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
        return AvailablePoints[Random.Range(0, AvailablePoints.Count)];
    }
}
