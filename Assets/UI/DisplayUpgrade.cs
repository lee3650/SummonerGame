using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUpgrade : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Button UpgradeButton;

    UpgradePath path;
    PlayerSummonController PlayerSummonController;

    public void ShowUpgrade(UpgradePath p, PlayerSummonController playerSummonController)
    {
        path = p;
        PlayerSummonController = playerSummonController;
        UpgradeButton.onClick.AddListener(delegate { UpgradeSummonButtonPressed(); });
        text.text = p.GetNextSummonStats();
    }

    void UpgradeSummonButtonPressed()
    {
        PlayerSummonController.UpgradeSummon(path);
    }
}