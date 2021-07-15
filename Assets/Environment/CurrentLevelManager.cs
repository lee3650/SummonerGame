using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class CurrentLevelManager : MonoBehaviour
{
    [SerializeField] WaveGenerator WaveGenerator;
    [SerializeField] LevelGenerator LevelGenerator;
    [SerializeField] WaveViewModel WaveViewModel;
    [SerializeField] WaveSpawner WaveSpawner;

    private int levelNum = 0;
    private int highestWave = 0; //so, this is actually super scuffed - if we change this it's going to break NextWaveFunctionMonitor. So, idk, watch out. 
    private int currentWave = 0; 

    private int baseEnemies = 2; 

    List<List<GameObject>> LevelWaves = new List<List<GameObject>>();

    public void EnterNextLevel()
    {
        LevelGenerator.GenerateNextLevel(levelNum);

        highestWave = Mathf.RoundToInt(Mathf.Lerp(3, 5, LevelGenerator.LevelPercentage(levelNum)));

        currentWave = 0;

        LevelWaves = new List<List<GameObject>>();

        for (int i = 0; i <= highestWave; i++)
        {
            LevelWaves.Add(WaveGenerator.GenerateNextWave(Mathf.RoundToInt(baseEnemies * GetWaveModifier(i/(highestWave - 1)))));
        }

        levelNum++;
    }

    public void SpawnNextWave()
    {
        print("Spawned wave: " + currentWave);
        WaveSpawner.SpawnWave(GetNextWave());
        currentWave++;
    }

    public List<GameObject> GetNextWave()
    {
        return LevelWaves[currentWave];
    }
    
    public bool OnLastWave()
    {
        return currentWave == highestWave;
    }

    public int GetHighestWave()
    {
        return highestWave;
    }
    public int GetCurrentWave()
    {
        return currentWave;
    }

    float GetWaveModifier(float wavePercentage)
    {
        return 0.75f * wavePercentage * Mathf.Cos(6 * wavePercentage) + 1;
    }
}
