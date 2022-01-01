using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharmDisplayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CharmText;
    [SerializeField] RewardManager RewardManager;
    [SerializeField] GameObject CharmPanel;

    public void ShowAllCharms()
    {
        CharmText.text = RewardManager.GetAllRewardsText();
        CharmPanel.SetActive(true);
    }
}
