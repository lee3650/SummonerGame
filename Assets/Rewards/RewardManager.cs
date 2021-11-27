using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [SerializeField] List<Reward> AllRewards;
    [SerializeField] float LevelEndRewardThreshold = 5.5f;

    [SerializeField] List<Reward> ExtraRewards;

    //okay so lower quality is better now. So, I guess our threshold should be like, 5 or 6. 
    //5 is kind of a high threshold lol. 5x more likely? 

    private void Awake()
    {
        foreach (Reward w in ExtraRewards)
        {
            if (ResearchManager.ResearchUnlocked(w.UnlockedIndex))
            {
                AllRewards.Add(w);
            }
        }
    }

    public Reward ChooseRandomRewardOnWaveEnd(bool excludeWorseRewards)
    {
        List<Reward> rewards = AllRewards;

        if (excludeWorseRewards)
        {
            //this means the level just ended 
            rewards = GetRewardsWithBetterQuality(LevelEndRewardThreshold);
        }

        AdjustRewardOdds(rewards);
        return ChooseRewardBasedOnCumulativeProbability(rewards);
    }

    public void ApplyAndProcessReward(Reward r)
    {
        ApplyReward(r);
        RemoveRewardIfNecessary(r);
        AddFollowingRewards(r);
    }

    private Reward ChooseRewardBasedOnCumulativeProbability(List<Reward> rewards)
    {
        //we may want to consider making this a utility function since I do it elsewhere. 
        float random = Random.Range(0f, 100f);

        if (random == 0)
        {
            return rewards[0]; //idk if this is even possible but since minScore is exclusive we should do it
        }

        for (int i = 0; i < rewards.Count; i++)
        {
            if (random > rewards[i].MinScore && random <= rewards[i].MaxScore)
            {
                return rewards[i];
            }
        }

        throw new System.Exception("Could not find reward given random number! Random: " + random);
    }

    private void AdjustRewardOdds(List<Reward> rewards)
    {
        float sum = 0;
        foreach (Reward r in rewards)
        {
            sum += r.Quality; //eh... yeah I guess that makes sense. 
        }

        //okay so sum is initialized. Now what's the equation? Percent chance (width or delta of MaxScore - MinScore) = Quality/Sum 
        //alright. 

        if (rewards.Count == 0)
        {
            throw new System.Exception("There were no possible rewards!");
        }

        rewards[0].MinScore = 0f;
        rewards[0].MaxScore = 100f * (rewards[0].Quality / sum);

        for (int i = 1; i < rewards.Count; i++)
        {
            rewards[i].MinScore = rewards[i - 1].MaxScore;
            rewards[i].MaxScore = (100f * (rewards[i].Quality / sum)) + rewards[i].MinScore;
        }
    }

    private void ApplyReward(Reward reward)
    {
        reward.ApplyReward();
    }

    private void RemoveRewardIfNecessary(Reward reward)
    {
        if (!reward.Reusable)
        {
            AllRewards.Remove(reward);
        }
    }

    void AddFollowingRewards(Reward reward)
    {
        foreach (Reward r in reward.FollowingRewards)
        {
            AllRewards.Add(r);
        }
    }

    List<Reward> GetRewardsWithBetterQuality(float quality) //better means lower 
    {
        List<Reward> result = new List<Reward>();

        foreach (Reward r in AllRewards)
        {
            if (r.Quality < quality)
            {
                result.Add(r);
            }
        }

        return result; 
    }
}
