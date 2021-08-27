using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedUpgradeManager : MonoBehaviour
{
    [SerializeField] private List<UpgradePath> UnlockedUpgrades = new List<UpgradePath>();

    public void UnlockUpgrade(UpgradePath upgrade)
    {
        UnlockedUpgrades.Add(upgrade);
    }

    public List<UpgradePath> GetUnlockedUpgrades(SummonType type, int tier)
    {
        List<UpgradePath> upgrades = new List<UpgradePath>();

        for (int i = UnlockedUpgrades.Count - 1; i >= 0; i--)
        {
            if (UnlockedUpgrades[i].SummonType == type && UnlockedUpgrades[i].Tier == tier)
            {
                upgrades.Add(UnlockedUpgrades[i]);
            }
        }

        return upgrades; 
    }
}
