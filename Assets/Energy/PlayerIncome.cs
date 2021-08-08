using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIncome : MonoBehaviour, IWaveNotifier
{
    [SerializeField] float IncomePerWave = 10f;
    [SerializeField] Summoner PlayerSummoner;

    public void OnWaveEnds()
    {
        PlayerSummoner.AddMana(IncomePerWave);
    }

    public void AddIncome(float extra)
    {
        IncomePerWave += extra; 
    }
}
