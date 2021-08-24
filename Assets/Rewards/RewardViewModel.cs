using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardViewModel : MonoBehaviour, IWaveNotifier
{
    [SerializeField] RewardManager RewardManager;
    [SerializeField] float WaveEndsRewardChance = 15f;
    [SerializeField] float LevelEndsRewardChance = 98f;
    [SerializeField] CurrentLevelManager CurrentLevelManager;
    [SerializeField] RewardPanel RewardPanel;

    float curChance; 

    void Start()
    {
        WaveSpawner.NotifyWhenWaveEnds(this);
        curChance = WaveEndsRewardChance;
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnWaveEnds();
        }
         */
    }

    public void WonSpin()
    {
        Reward reward = RewardManager.ChooseRandomRewardOnWaveEnd(CurrentLevelManager.OnLastWave());
        RewardManager.ApplyAndProcessReward(reward);
        RewardPanel.WonReward(reward);
    }

    public void OnWaveEnds()
    {
        UpdateCurChance();
        RewardPanel.Show(curChance);
    }

    void UpdateCurChance()
    {
        if (CurrentLevelManager.OnLastWave())
        {
            curChance = LevelEndsRewardChance;
        }
        curChance = WaveEndsRewardChance;
    }
}
