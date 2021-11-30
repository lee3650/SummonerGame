using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchHider : MonoBehaviour
{
    //we can make this more generic but we don't really need to right now
    [SerializeField] GameObject[] FirstHiddenUnlocks;
    [SerializeField] GameObject[] LastHiddenUnlocks;
    [SerializeField] ResearchManager ResearchManager;
    [SerializeField] LastLetterPanel LastLetterPanel;
    [SerializeField] GameObject AdvancedResearchPanel;
    [SerializeField] GameObject WarshipsPanel;

    private void Awake()
    {
        SetFirstUnlocksVis(false);
        SetLastUnlocksVis(false);

        ResearchManager.FinishedResearch += FinishedResearch;
    }

    private void FinishedResearch(int index)
    {
        if (ResearchManager.NumberOfUnlockedResearch() == ResearchManager.NumOfFirstResearches)
        {
            AdvancedResearchPanel.SetActive(true);
        } 

        if (ResearchManager.NumberOfUnlockedResearch() == ResearchManager.LastResearchIndex)
        {
            WarshipsPanel.SetActive(true);
        }
        
        if (index == ResearchManager.LastResearchIndex)
        {
            LastLetterPanel.Show();
        }
    }

    private void SetFirstUnlocksVis(bool vis)
    {
        foreach (GameObject g in FirstHiddenUnlocks)
        {
            g.SetActive(vis);
        }
    }
    private void SetLastUnlocksVis(bool vis)
    {
        foreach (GameObject g in LastHiddenUnlocks)
        {
            g.SetActive(vis);
        }
    }

    private void Update()
    {
        if (ResearchManager.NumberOfUnlockedResearch() >= ResearchManager.NumOfFirstResearches)
        {
            SetFirstUnlocksVis(true);
        }
        if (ResearchManager.NumberOfUnlockedResearch() >= ResearchManager.LastResearchIndex)
        {
            SetLastUnlocksVis(true);
        }
    }
}
