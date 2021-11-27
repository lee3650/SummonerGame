using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OverflowNotice : MonoBehaviour
{
    [SerializeField] ResearchManager ResearchManager;
    [SerializeField] TextMeshProUGUI OverflowString;

    private string prevResearch = "";

    private void Awake()
    {
        ResearchManager.FinishedResearch += FinishedResearch;
    }
    
    private void Update()
    {
        if (ResearchManager.Overflow != null)
        {
            OverflowString.text = "Our study of " + prevResearch + " has given us some ideas about " + ResearchManager.Overflow.name;
        } else
        {
            OverflowString.text = "";
        }
    }

    private void FinishedResearch(int index)
    {
        prevResearch = ResearchManager.GetResearchName(index);
    }
}
