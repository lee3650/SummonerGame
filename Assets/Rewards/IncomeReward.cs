using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomeReward : Reward
{
    [SerializeField] PlayerIncome PlayerIncome;
    [SerializeField] float MinIncrease, MaxIncrease;

    float incomeAmt;

    public override void ApplyReward()
    {
        incomeAmt = Random.Range(MinIncrease, MaxIncrease);
        PlayerIncome.AddIncome(incomeAmt);
    }

    public override string Description
    {
        get
        {
            return "Income increased by " + incomeAmt;
        }
    }
}
