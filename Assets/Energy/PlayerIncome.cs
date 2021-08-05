using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIncome : MonoBehaviour, IWaveNotifier
{
    [SerializeField] float IncomePerWave = 10f;
    [SerializeField] Summoner PlayerSummoner; 

    private void Awake()
    {
        WaveSpawner.NotifyWhenWaveEnds(this); 
    }

    public void OnWaveEnds()
    {
        PlayerSummoner.AddMana(IncomePerWave);
    }

    public void AddIncome(float extra)
    {
        IncomePerWave += extra; 
    }
}
