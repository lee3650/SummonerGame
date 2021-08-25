using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyReward : Reward
{
    [SerializeField] ManaManager PlayerManaManager;
    [SerializeField] float min, max;

    float rewardAmt; 

    public override void ApplyReward()
    {
        rewardAmt = Random.Range(min, max);
        rewardAmt = Mathf.Round(rewardAmt);
        PlayerManaManager.IncreaseMana(rewardAmt);
    }

    public override string Description
    {
        get
        {
            return rewardAmt + " mana";
        }
    }
}
