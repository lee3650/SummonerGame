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

        TryPlayGif(DisplayRewardData);

        DisplayText(DisplayRewardData);
    }

    private void TryPlayGif(DisplayRewardData displayRewardData)
    {
        if (displayRewardData.HasGif)
        {
            GifDisplay.PlayGif(displayRewardData.Gif);
        }
    }

    public void ShowPreview(DisplayRewardData data)
    {
        TryPlayGif(data);
    }

    private void DisplayText(DisplayRewardData data)
    {
        string path = MainMenuScript.appendPath + data.TextPath;

        if (File.Exists(path))
        {
            Text.text = StringWidthFormatter.FormatStringToWidth(File.ReadAllText(path), 110);
        }
        else
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
