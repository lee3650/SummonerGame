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
    
    StageNode HeadNode;
    StageNode RootNode; 

    public void EnterFirstLevel()
    {
        LevelGenerator.SetTotalMapSizeAndInitMap();
        
        HeadNode = new StageNode(new Vector2(0, 1), null);
        RootNode = HeadNode;

        LevelGenerator.GenerateNextLevel(levelNum, HeadNode.Position, Vector2.right);
        Player.position = MapManager.GetClosestValidTile(Player.position);
    }

    public void EnterNextLevel()
    {
        WaveGenerator.AddSpawns(AdditionalSpawns[levelNum].Spawns);

        if (ShouldGenerateLevel(levelNum))
        {
            HeadNode = GenerateNextNode(HeadNode, levelNum);
            LevelGenerator.GenerateNextLevel(levelNum, HeadNode.Position, HeadNode.Delta);
            LevelGenerator.RecalculateSpawnRegion(FindEndNodes(RootNode));
        }

        highestWave = Mathf.RoundToInt(Mathf.Lerp(3, 5, LevelGenerator.LevelPercentage(levelNum)));

        currentWave = 0;

        LevelWaves = new List<List<GameObject>>();

        for (int i = 0; i <= highestWave; i++)
        {
            LevelWaves.Add(WaveGenerator.GenerateNextWave(Mathf.RoundToInt(baseEnemies * GetWaveModifier(i / (highestWave - 1)))));
        }

        HealPlayer();

        baseEnemies = LevelWaves[LevelWaves.Count - 2].Count; //this is kind of confusing code, but it generates an additional wave past the highest wave because the UI always needs a next wave 

        levelNum++;
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
            return new StageNode(new Vector2(1, 1), node);
        }

        Vector2[] dirs = new Vector2[]
        {
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(0, -1),
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
        return point.x < LevelGenerator.MapStagesWidth && point.x >= 0 && point.y >= 0 && point.y < LevelGenerator.MapStagesHeight;
    }

    private bool ShouldGenerateLevel(int level)
    {
        switch (level)
        {
            case 0:
            case 1: 
            case 3:
            case 5:
            case 7: 
                return true;
        }
        return false; 
    }

    public void SpawnNextWave()
    {
        print("Spawned wave: " + currentWave);
        WaveSpawner.SpawnWave(GetNextWave());
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
    
    public int GetLevelNum()
    {
        return levelNum;
    }

    float GetWaveModifier(float wavePercentage)
    {
        return 0.75f * wavePercentage * Mathf.Cos(6 * wavePercentage) + 1;
    }
}
