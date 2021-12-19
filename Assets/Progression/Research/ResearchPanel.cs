using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class ResearchPanel : MonoBehaviour
{
    [SerializeField] VideoPlayer Video;
    [SerializeField] TextMeshProUGUI ResearchName;
    [SerializeField] TextMeshProUGUI ResearchDescription;
    [SerializeField] TextMeshProUGUI ResearchRequirements;
    [SerializeField] TextMeshProUGUI Islands;
    [SerializeField] TextMeshProUGUI ResearchProgress;
    [SerializeField] ResearchManager ResearchManager;
    [SerializeField] Image Image;
    [SerializeField] Button SetResearch;
    [SerializeField] TextMeshProUGUI tooltip;
    [SerializeField] XPApplier XPApplier;

    private int curIndex;

    public void Show(Research research, bool unlocked) //so... somehow, we need to figure out if it's just a letter, right. 
    {
        Video.clip = research.Gif;
        Video.Play(); //does this start it over too? 
        ResearchName.text = research.name;

        if (unlocked)
        {
            ResearchName.text = "Unlocked " + research.name + "!";
        }

        ResearchDescription.text = research.Description;
        ResearchProgress.text = string.Format("{0}/{1}", research.Progress, research.XPReq);
        ResearchRequirements.text = "XP: " + research.XPReq;
        curIndex = research.Index;
        Islands.text = "Islands: " + research.TotalIslands;
        Image.sprite = research.Image;

        SetResearch.interactable = research.PrereqUnlocked && !research.Unlocked;
        if (research.Unlocked)
        {
            tooltip.text = "You already researched this!";
        } else if (!research.PrereqUnlocked)
        {
            tooltip.text = "Research " + research.Prereq.name + " first!";
        } else
        {
            tooltip.text = "";
        }

        gameObject.SetActive(true);
    }

    public void SetCurrentResearch()
    {
        ResearchManager.SetCurrentResearch(curIndex);

        gameObject.SetActive(false);
    }

    public void Hide()
    {
        XPApplier.AllRewardPanelsClosed();
        gameObject.SetActive(false);
    }
}
