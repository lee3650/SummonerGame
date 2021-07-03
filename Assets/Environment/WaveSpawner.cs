﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] PlayerEntity PlayerEntity;
    [SerializeField] List<GameObject> Enemies;

    private int waveNum = 1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnEnemies(2 * waveNum);    
            waveNum++; 
        }
    }

    void SpawnEnemies(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject enemy = Instantiate(Enemies[Random.Range(0, Enemies.Count)], (Vector2)transform.position + Random.insideUnitCircle * 3, Quaternion.Euler(Vector3.zero));
            enemy.GetComponent<AIEntity>().WakeUp();
            enemy.GetComponent<TargetManager>().Target = PlayerEntity;
        }
    }

}