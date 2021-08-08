﻿using System.Collections;
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
            enemy.GetComponent<AIEntity>().WakeUp();
            CurrentWave.Add(enemy.GetComponent<AIEntity>());
        }

        print("Spawned entities!");

        NotifiedWaveCompletion = false;
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
