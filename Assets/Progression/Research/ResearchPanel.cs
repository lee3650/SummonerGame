using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResearchPanel : MonoBehaviour
{
    [SerializeField] GifDisplayer GifDisplayer;
    [SerializeField] TextMeshProUGUI ResearchName;
    [SerializeField] TextMeshProUGUI ResearchDescription;
    [SerializeField] TextMeshProUGUI ResearchRequirements;
    [SerializeField] TextMeshProUGUI Islands;
    [SerializeField] TextMeshProUGUI ResearchProgress;
    [SerializeField] ResearchManager ResearchManager;
    [SerializeField] Image Image;
    [SerializeField] Button SetResearch;
    [SerializeField] TextMeshProUGUI tooltip;

    private int curIndex;

    public void Show(Research research)
    {
        GifDisplayer.PlayGif(research.Gif);
        ResearchName.text = research.name;
        ResearchDescription.text = research.Description;
        ResearchProgress.text = string.Format("{0}/{1}", research.Progress, research.Kills);
        ResearchRequirements.text = "Kills: " + research.Kills;
        curIndex = research.Index;
        Islands.text = "Islands: " + research.TotalIslands;
        Image.sprite = research.Image;

        SetResearch.interactable = research.PrereqUnlocked;
        if (!research.PrereqUnlocked) 
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
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
