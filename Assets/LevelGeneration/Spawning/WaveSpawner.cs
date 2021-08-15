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

    static List<IWaveNotifier> ClientsToNotify = new List<IWaveNotifier>(); 

    List<Vector2> SpawnRegion = new List<Vector2>();

    List<AIEntity> CurrentWave = new List<AIEntity>();

    bool NotifiedWaveCompletion = true;

    public static bool IsCurrentWaveDefeated = true; 

    private void Awake()
    {
        if (UseTransforms)
        {
            SpawnRegion = LevelGenerator.GetPointsWithinBoundaries(BottomLeft.position, TopRight.position);
        }
    }

    public void AddSpawnRegion(List<Vector2> region)
    {
        SpawnRegion.AddRange(region);
    }

    public static void NotifyWhenWaveEnds(IWaveNotifier notifier)
    {
        ClientsToNotify.Insert(0, notifier);
    }

    public static void StopNotifyingWhenWaveEnds(IWaveNotifier notifier)
    {
        ClientsToNotify.Remove(notifier);
    }

    public void SpawnWave(List<GameObject> wave)
    {
        CurrentWave = new List<AIEntity>();

        for (int i = 0; i < wave.Count; i++)
        {
            GameObject enemy = Instantiate(wave[i], GetRandomPointInSpawnZone(), Quaternion.Euler(Vector3.zero));
            //enemy.GetComponent<AIEntity>().WakeUp();
            CurrentWave.Add(enemy.GetComponent<AIEntity>());
        }

        StartCoroutine(EnableEnemies());

        print("Spawned entities!");

        NotifiedWaveCompletion = false;
        IsCurrentWaveDefeated = false; 
    }

    private IEnumerator EnableEnemies()
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

            yield return new WaitForSeconds(0.75f);
        }
    }

    private void LateUpdate()
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
        return SpawnRegion[Random.Range(0, SpawnRegion.Count)]; 
    }
}
