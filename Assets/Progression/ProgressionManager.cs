using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    [SerializeField] LevelUnlocks[] ProgressionRewards; //so, index = level unlocked
    private static LevelUnlocks[] StaticProgRewards = new LevelUnlocks[0];
    private static Dictionary<GameplayChange, bool> GameplayChangePairs = new Dictionary<GameplayChange, bool>();

    private void Awake()
    {
        ExperienceManager.LeveledUp += LeveledUp;
        StaticProgRewards = ProgressionRewards;
        SetGameplayChangePairs(0, ExperienceManager.GetCurrentLevel());
    }

    public static void ResetState()
    {
        SetGameplayChangePairs(0, 10);
    }

    private static void SetGameplayChangePairs(int start, int level)
    {
        for (int i = start; i <= level; i++)
        {
            ProgressionUnlock[] unlocks = UnlocksFromLevel(i);
            foreach (ProgressionUnlock u in unlocks)
            {
                if (u.SetGameplayChange)
                {
                    GameplayChangePairs[u.ChangeToSet] = true;
                }
            }
        }
    }

    private void LeveledUp()
    {
        SetGameplayChangePairs(ExperienceManager.GetCurrentLevel(), ExperienceManager.GetCurrentLevel());
    }

    public static ProgressionUnlock[] UnlocksFromLevel(int level)
    {
        if (level >= StaticProgRewards.Length)
        {
            return new ProgressionUnlock[0];
        }

        return StaticProgRewards[level].Unlocks;
    }

    public ProgressionUnlock[] GetUnlocksFromLevel(int level)
    {
        return UnlocksFromLevel(level);
    }

    public List<DisplayRewardData> GetRewardDataAtLevel(int level) 
    {
        if (level >= ProgressionRewards.Length)
        {
            return new List<DisplayRewardData>();
        }

        return ProgressionRewards[level].GetRewardData();
    }

    public static bool UseGameplayChange(GameplayChange change)
    {
        bool result = false;
        if (GameplayChangePairs.TryGetValue(change, out result))
        {
            return result;
        }
        return false;
    }

    public static List<GameObject> GetItemsUnlockedAtLevel(int level)
    {
        if (StaticProgRewards.Length == 0)
        {
            return new List<GameObject>();
        }

        List<GameObject> result = new List<GameObject>();

        for (int i = 0; i <= level; i++)
        {
            if (i < StaticProgRewards.Length)
            {
                result.AddRange(StaticProgRewards[i].GetUnlockedItems());
            }
        }

        return result; 
    }

    private void OnDestroy()
    {
        ExperienceManager.LeveledUp -= LeveledUp; 
    }
}
