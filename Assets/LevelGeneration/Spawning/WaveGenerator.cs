using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour
{
    [SerializeField] List<GameObject> Enemies; 

    public List<GameObject> GenerateNextWave(int num)
    {
        List<GameObject> NextWave = new List<GameObject>();

        for (int i = 0; i < num; i++)
        {
            NextWave.Add(Enemies[Random.Range(0, Enemies.Count)]);
        }

        return NextWave;
    }
}
