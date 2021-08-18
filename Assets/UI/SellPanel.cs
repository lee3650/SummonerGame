using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellPanel : UIPanel
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Button UpgradeButton;

    Sellable sellable;
    PlayerSummonController PlayerSummonController;

    public override void Show(object input)
    {
        (Sellable, PlayerSummonController)? castInput = input as (Sellable, PlayerSummonController)?; //okay that's a little sketchy. Maybe more than a little.  

        sellable = castInput.Value.Item1;
        PlayerSummonController = castInput.Value.Item2; //ewww lol 
        UpgradeButton.onClick.AddListener(delegate { UpgradeSummonButtonPressed(); });
        text.text = sellable.GetSellText();
    }

    void UpgradeSummonButtonPressed()
    {
        PlayerSummonController.SellSummon(sellable);
    }
}
