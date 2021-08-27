using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeReward : Reward
{
    [SerializeField] UpgradePath UnlockedUpgrade;
    [SerializeField] UnlockedUpgradeManager UnlockedUpgradeManager;

    public override void ApplyReward()
    {
        UnlockedUpgradeManager.UnlockUpgrade(UnlockedUpgrade);
    }
}
