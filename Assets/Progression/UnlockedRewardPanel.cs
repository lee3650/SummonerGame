using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockedRewardPanel : UIPanel
{

    RewardPanelShower rds;

    public override void Show(object information)
    {
        DisplayRewardData DisplayRewardData = information as DisplayRewardData;
        rds = DisplayRewardData.RewardPanelShower;
    }

    public void HidePanel()
    {
        rds.HideRewardPanel(this);
        Destroy(gameObject); //?
    }
}
