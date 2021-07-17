using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour
{
    [SerializeField] List<SpawnToProbability> Enemies; 

    public List<GameObject> GenerateNextWave(int num)
    {
        List<GameObject> NextWave = new List<GameObject>();

        for (int i = 0; i < num; i++)
        {
            GameObject enemy = ChooseRandomEnemy(); 

            NextWave.Add(enemy);
        }

        return NextWave;
    }

    GameObject ChooseRandomEnemy()
    {
        float random = Random.Range(0, 100f);

        foreach (SpawnToProbability s in Enemies)
        {
            if (random <= s.Likelihood)
            {
                return s.Spawn;
            }
        }
        throw new System.Exception("Could not select random enemy!");
    }
}
