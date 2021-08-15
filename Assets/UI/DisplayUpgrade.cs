using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayUpgrade : UIPanel
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Button UpgradeButton;

    UpgradePath path;
    PlayerSummonController PlayerSummonController;

    public override void Show(object input)
    {
        (UpgradePath, PlayerSummonController)? castInput = input as (UpgradePath, PlayerSummonController)?; //okay that's a little sketchy. Maybe more than a little.  

        path = castInput.Value.Item1;
        PlayerSummonController = castInput.Value.Item2; //ewww lol 
        UpgradeButton.onClick.AddListener(delegate { UpgradeSummonButtonPressed(); });
        text.text = path.GetNextSummonStats();
    }

    void UpgradeSummonButtonPressed()
    {
        PlayerSummonController.UpgradeSummon(path);
    }
}