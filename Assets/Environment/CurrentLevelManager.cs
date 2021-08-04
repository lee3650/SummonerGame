using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class CurrentLevelManager : MonoBehaviour
{
    [SerializeField] WaveGenerator WaveGenerator;
    [SerializeField] LevelGenerator LevelGenerator;
    [SerializeField] WaveViewModel WaveViewModel;
    [SerializeField] WaveSpawner WaveSpawner;
    [SerializeField] Transform Player;
    [SerializeField] List<SpawnToOddsWrapper> AdditionalSpawns;

    private int levelNum = 0;
    private int highestWave = 0; //so, this is actually super messed up - if we change this it's going to break NextWaveFunctionMonitor. So, idk, watch out. 
    private int currentWave = 0; 

    private int baseEnemies = 3; 

    List<List<GameObject>> LevelWaves = new List<List<GameObject>>();

    LevelDirections[] GenerationTemplate = new LevelDirections[]
    {
        LevelDirections.East,
        LevelDirections.DoNotGenerate,
        LevelDirections.West,
        LevelDirections.DoNotGenerate,
        LevelDirections.North,
        LevelDirections.DoNotGenerate,
        LevelDirections.South,
        LevelDirections.DoNotGenerate,
        LevelDirections.DoNotGenerate,
        LevelDirections.DoNotGenerate,
        LevelDirections.DoNotGenerate,
    };

    public void EnterFirstLevel()
    {
        LevelGenerator.SetTotalMapSizeAndInitMap();
        LevelGenerator.GenerateNextLevel(levelNum, LevelDirections.Center);
        Player.position = MapManager.GetClosestValidTile(Player.position);
    }

    public void EnterNextLevel()
    {
        WaveGenerator.AddSpawns(AdditionalSpawns[levelNum].Spawns);

        if (levelNum < GenerationTemplate.Length && GenerationTemplate[levelNum] != LevelDirections.DoNotGenerate)
        {
            LevelGenerator.GenerateNextLevel(levelNum, GenerationTemplate[levelNum]);
        }

        highestWave = Mathf.RoundToInt(Mathf.Lerp(3, 5, LevelGenerator.LevelPercentage(levelNum)));

        currentWave = 0;

        LevelWaves = new List<List<GameObject>>();

        for (int i = 0; i <= highestWave; i++)
        {
            LevelWaves.Add(WaveGenerator.GenerateNextWave(Mathf.RoundToInt(baseEnemies * GetWaveModifier(i/(highestWave - 1)))));
        }

        baseEnemies = LevelWaves[LevelWaves.Count - 2].Count; //this is kind of confusing code, but it generates an additional wave past the highest wave because the UI always needs a next wave 
        
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
    
    public int GetLevelNum()
    {
        return levelNum;
    }

    float GetWaveModifier(float wavePercentage)
    {
        return 0.75f * wavePercentage * Mathf.Cos(6 * wavePercentage) + 1;
    }
}
