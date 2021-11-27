using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SetResearchProgress : MonoBehaviour
{
    [SerializeField] GameObject progressMenu;
    [SerializeField] TextMeshProUGUI chooseText;
    [SerializeField] TextMeshProUGUI ProgressText;
    [SerializeField] Image RewardSprite;
    [SerializeField] TextMeshProUGUI IslandsLeft;
    [SerializeField] ResearchManager ResearchManager;

    private void Update()
    {
        if (ResearchManager.CurrentResearch != null)
        {
            progressMenu.SetActive(true);
            chooseText.gameObject.SetActive(false);

            ProgressText.text = string.Format("{0}/{1}", ResearchManager.CurrentResearch.Progress, ResearchManager.CurrentResearch.XPReq);
            RewardSprite.sprite = ResearchManager.CurrentResearch.Image;
            IslandsLeft.text = "Islands Left: " + ResearchManager.CurrentResearch.IslandsLeft;
        } else
        {
            progressMenu.SetActive(false);
            chooseText.gameObject.SetActive(true);

            if (ResearchManager.AllResearchUnlocked())
            {
                chooseText.text = "Research Complete!";
            }
        }
    }
}
