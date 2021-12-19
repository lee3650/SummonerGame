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
    [SerializeField] OceanGenerator OceanGenerator;
    [SerializeField] MapDrawer MapDrawer;

    private const int maxLevel = 7;
    private int levelNum = 0;
    private int highestWave = 0; //so, this is actually super messed up - if we change this it's going to break NextWaveFunctionMonitor. So, idk, watch out. 
    private int currentWave = 0;
    private float enemySpawnTime = 0.75f;
    private int baseEnemies = 3;

    List<List<GameObject>> LevelWaves = new List<List<GameObject>>();
    List<int> firstEnemyRoll = new List<int>();
    List<int> secondEnemyRoll = new List<int>();
    private int previousBaseEnemies;

    StageNode HeadNode;
    StageNode RootNode;

    private void Awake()
    {
        List<SpawnToProbability> extraSpawns = LetterManager.GetExtraSpawns();

        if (extraSpawns == null)
        {
            return;
        }

        for (int i = 0; i < extraSpawns.Count; i++)
        {
            AdditionalSpawns[extraSpawns[i].startingLevel].Spawns.Add(extraSpawns[i]);
        }
    }

    public void EnterFirstLevel()
    {
        BridgeAdder.ResetBridges();

        LevelGenerator.SetTotalMapSizeAndInitMap();

        LevelGenerator.GenerateNextLevel();

        LevelGenerator.RecalculateSpawnRegion();

        MapDrawer.ConditionallyDestroyTiles();

        OceanGenerator.DrawOcean();

        BridgeAdder.WriteBridges();

        ContinuityManager.CalculateContinuity(MapManager.GetMap());
    }

    public void EnterNextLevel()
    {
        WaveGenerator.AddSpawns(AdditionalSpawns[levelNum].Spawns);

        /*
        if (ShouldGenerateLevel(levelNum))
        {
            HeadNode = GenerateNextNode(HeadNode, levelNum);
            LevelGenerator.GenerateNextLevel(levelNum, HeadNode.Position, HeadNode.Delta);
            LevelGenerator.RecalculateSpawnRegion(FindEndNodes(RootNode));
        }
         */

        highestWave = Mathf.RoundToInt(Mathf.Lerp(3, 5, LevelGenerator.LevelPercentage(levelNum)));

        currentWave = 0;

        //previousBaseEnemies = baseEnemies;

        LevelWaves = new List<List<GameObject>>();
        firstEnemyRoll = new List<int>();
        secondEnemyRoll = new List<int>();

        for (int i = 0; i <= highestWave; i++)
        {
            int roll = GetEnemyNumber(i, levelNum);
            int roll2 = GetEnemyNumber(i, levelNum);
            firstEnemyRoll.Add(roll);
            secondEnemyRoll.Add(roll2);

            print(string.Format("level: {0}, roll between {1} and {2} for wave {3}; first roll: {4}, second: {5}", levelNum, GetEnemiesMinRoll(i, levelNum), GetEnemiesPerWaveMaxRoll(i, levelNum), i, roll, roll2));

            LevelWaves.Add(WaveGenerator.GenerateNextWave(roll + roll2));
        }

        HealPlayer();

        baseEnemies = CalculateBaseEnemies(levelNum);

        SetSpawnTime();

        levelNum++;
    }

    private int CalculateBaseEnemies(int level)
    {
        int[] baseEnemies = new int[]
        {
            5,
            8,
            12,
            18,
            25,
            32,
        };

        return baseEnemies[level];
    }

    private int GetEnemiesPerWaveMaxRoll(int wave, int level)
    {
        int actual = 4 * ((level + 1) * (level + 1)) + 4;
        int prev = 4 * (level * level) + 4;
        return Mathf.RoundToInt(Mathf.Lerp((float)prev, (float)actual, (float)wave/highestWave));
    }

    public int GetEnemiesMinRoll(int wave, int level)
    {
        int actual = (((level + 1 ) * (level + 1)) * 2) + 1;
        int prev = (2 * (level * level)) + 1;
        return Mathf.RoundToInt(Mathf.Lerp((float)prev, (float)actual, (float)wave / highestWave));
    }

    public int GetFirstRoll()
    {
        return firstEnemyRoll[currentWave];
    }

    public int GetSecondRoll()
    {
        return secondEnemyRoll[currentWave - 1];
    }

    public string GetNextWaveDescription()
    {
        print("next wave description for wave " + currentWave + ", levelNum " + (levelNum - 1) + string.Format(" is between {0} and {1}", GetEnemiesMinRoll(currentWave, levelNum - 1), GetEnemiesPerWaveMaxRoll(currentWave, levelNum - 1)));
        return string.Format("Scouts found: {0} enemies\nPlus between {1} and {2} more", GetFirstRoll(), GetEnemiesMinRoll(currentWave, levelNum - 1), GetEnemiesPerWaveMaxRoll(currentWave, levelNum - 1) - 1);
    }

    public int GetEnemyNumber(int wave, int level)
    {
        return Random.Range(GetEnemiesMinRoll(wave, level), GetEnemiesPerWaveMaxRoll(wave, level));
    }

    private void SetSpawnTime()
    {
        enemySpawnTime = Mathf.Lerp(0.75f, 0.1f, ((float)levelNum + 1) / maxLevel); //Mathf.Pow(((float)(levelNum + 1)/ maxLevel), 2));
        print("Spawn time: " + enemySpawnTime);
    }

    List<StageNode> FindEndNodes(StageNode root)
    {
        List<StageNode> result = new List<StageNode>();

        foreach (StageNode n in root.Children)
        {
            if (n.Children.Count == 0)
            {
                result.Add(n);
            }
        }

        //this is kind of a weird way to do this but I guess it works. 
        //I guess we could pass state through the list too. Hm. I feel like typically you'd only check yourself for children. 
        foreach (StageNode n in root.Children)
        {
            result.AddRange(FindEndNodes(n));
        }

        return result; 
    }

    StageNode GenerateNextNode(StageNode node, int levelNum)
    {
        if (node == null)
        {
            throw new System.Exception("Could not generate next node!"); //I'm not really expecting this to happen. 
        }

        if (levelNum == 0)
        {
            return new StageNode(new Vector2(1, 0), node);
        }

        Vector2[] dirs = new Vector2[]
        {
            new Vector2(1, 0),
            //new Vector2(0, 1), //uncomment if you want the world to go up and down. 
            //new Vector2(0, -1),
        };

        dirs = (Vector2[])ListRandomSort<Vector2>.SortListRandomly(dirs);

        for (int i = 0; i < dirs.Length; i++)
        {
            Vector2 next = node.Position + dirs[i];
            if (IsPointInTotalMapBounds(next) && IsThereStageAtPoint(next) == false) 
            {
                return new StageNode(next, node);
            }
        }

        return GenerateNextNode(node.Parent, levelNum);
    }

    bool IsThereStageAtPoint(Vector2 point)
    {
        return IsNodeOrChildrenAtPoint(RootNode, point);
    }

    bool IsNodeOrChildrenAtPoint(StageNode node, Vector2 point)
    {
        if (point == node.Position)
        {
            return true; 
        }

        //this is a little unusual, yeah?         
        foreach (StageNode n in node.Children)
        {
            if (IsNodeOrChildrenAtPoint(n, point))
            {
                return true; 
            }
        }

        return false; 
    }

    private bool IsPointInTotalMapBounds(Vector2 point)
    {
        return point.x < LevelGenerator.MapWidth && point.x >= 0 && point.y >= 0 && point.y < LevelGenerator.MapHeight;
    }

    public void SpawnNextWave()
    {
        print("Spawned wave: " + currentWave);

        float effectiveSpawnTime = enemySpawnTime;

        WaveSpawner.SpawnWave(GetNextWave(), effectiveSpawnTime);
        currentWave++;
    }

    public void HealPlayer()
    {
        int healAmt = Random.Range(10, 20);

        Player.GetComponent<HealthManager>().Heal(healAmt); //this is sketchy at best 
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

    public bool OnLastLevel()
    {
        return GetLevelNum() == 6; //hardcode in 6 I guess? 
    }

    public bool OnFinalWaveOfGame()
    {
        return OnLastWave() && OnLastLevel();
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
