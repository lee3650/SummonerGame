using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIncome : MonoBehaviour, IWaveNotifier
{
    [SerializeField] float IncomePerWave = 10f;
    [SerializeField] Summoner PlayerSummoner;

    public event Action IncomeChanged = delegate { };

    public void OnWaveEnds()
    {
        PlayerSummoner.AddMana(IncomePerWave);
    }

    public float GetIncome()
    {
        return IncomePerWave;
    }

    public void AddIncome(float extra)
    {
        IncomePerWave += extra;
        IncomeChanged();
    }
}
