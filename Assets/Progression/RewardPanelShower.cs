using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardPanelShower : MonoBehaviour
{
    [SerializeField] UnlockedRewardPanel RewardPrefab;
    [SerializeField] Transform PanelParent;
    [SerializeField] XPApplier XPApplier;
    [SerializeField] ProgressionManager ProgressionManager;

    private List<UnlockedRewardPanel> ShownPanels = new List<UnlockedRewardPanel>();

    public void ShowLevelRewards(int level)
    {
        List<DisplayRewardData> rewards = ProgressionManager.GetRewardDataAtLevel(level);

        foreach (DisplayRewardData data in rewards)
        {
            ShowRewardPanel(data);
        }

        if (rewards.Count == 0)
        {
            AllRewardsClosed();
        }        
    }

    private void AllRewardsClosed()
    {
        XPApplier.AllRewardPanelsClosed();
    }

    private void ShowRewardPanel(DisplayRewardData rd)
    {
        UnlockedRewardPanel panel = Instantiate<UnlockedRewardPanel>(RewardPrefab, PanelParent);
        rd.RewardPanelShower = this;
        panel.Show(rd);

        ShownPanels.Add(panel);
    }

    public void HideRewardPanel(UnlockedRewardPanel panel) //needs to take which panel is hidden
    {
        ShownPanels.Remove(panel);

    }
}
