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

        Text.text = File.ReadAllText(DisplayRewardData.TextPath);
    }

    public void HidePanel()
    {
        print("hid panel!");
        rds.HideRewardPanel(this);
        Destroy(gameObject);
    }
}
