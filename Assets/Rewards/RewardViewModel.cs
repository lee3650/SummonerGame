﻿using System.Collections;
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
        if (ShouldShowReward())
        {
            RewardPanel.Show(curChance);
        }
    }

    bool ShouldShowReward()
    {
        if (Random.Range(0, 100f) < curChance)
        {
            return true; 
        }
        return false; 
    }

    void UpdateCurChance()
    {
        if (CurrentLevelManager.OnLastWave())
        {

            curChance = LevelEndsRewardChance;
        } else
        {
            curChance = WaveEndsRewardChance;
        }
    }
}