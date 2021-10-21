using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerSummon : TileRestrictedSummon
{
    static float IncomeMultipler = 1.2f;
    static float CalculatedCost;

    public override float GetManaDrain()
    {
        return CalculatedCost;
    }

    public static void ScaleCost(Summoner summoner)
    {
        float income = summoner.CalculateIncome();
        print("scaling cost: income is " + income);
        CalculatedCost = RoundToHalf(income * IncomeMultipler);
    }

    private static float RoundToHalf(float input)
    {
        float whole = (int)input;
        float frac = input - whole;
        int tens = (int)(10 * frac);
        if (tens <= 2.5)
        {
            return whole;
        }
        if (tens >= 7.5)
        {
            return whole + 1;
        }
        return whole + 0.5f;
    }
}
