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

        List<SpawnToProbability> enabledEnemies = new List<SpawnToProbability>();

        //it's definitely more efficient to just update our calculated minimums/maximums only when the enabled enemies change 
        float sum = 0;
        foreach (SpawnToProbability s in Enemies)
        {
            if (s.Enabled)
            {
                sum += s.RelativeLikelihood;
            }
        }
        
        foreach (SpawnToProbability s in Enemies)
        {
            if (s.Enabled)
            {
                float percentChance = 100f * s.RelativeLikelihood / sum;
                if (enabledEnemies.Count == 0)
                {
                    s.CalculatedMinimum = 0f;
                    s.CalculatedMaximum = percentChance;
                } else
                {
                    s.CalculatedMinimum = enabledEnemies[0].CalculatedMaximum;
                    s.CalculatedMaximum = s.CalculatedMinimum + percentChance;
                }

                enabledEnemies.Insert(0, s);
            }
        }

        foreach (SpawnToProbability s in enabledEnemies)
        {
            if (random >= s.CalculatedMinimum && random <= s.CalculatedMaximum)
            {
                return s.Spawn;
            }
        }

        throw new System.Exception("Could not select random enemy!");
    }
}
