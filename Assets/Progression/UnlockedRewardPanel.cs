using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class UnlockedRewardPanel : UIPanel
{
    [SerializeField] TextMeshProUGUI Text;
    [SerializeField] GifDisplayer GifDisplay;

    RewardPanelShower rds;

    public override void Show(object information)
    {
        DisplayRewardData DisplayRewardData = information as DisplayRewardData;
        rds = DisplayRewardData.RewardPanelShower;

        if (DisplayRewardData.HasGif)
        {
            GifDisplay.PlayGif(DisplayRewardData.Gif);
        }

        if (File.Exists(DisplayRewardData.TextPath))
        {
            Text.text = File.ReadAllText(DisplayRewardData.TextPath);
        } else
        {
            Text.text = "";
        }
    }

    public void HidePanel()
    {
        print("hid panel!");
        if (rds != null)
        {
            rds.HideRewardPanel(this);
        }
        Destroy(gameObject);
    }
}
