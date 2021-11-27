using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideNextButton : MonoBehaviour
{
    [SerializeField] ResearchManager ResearchManager;
    [SerializeField] GameObject NextLevelButton;
    [SerializeField] IslandsLeftSetter IslandsLeftSetter;
    private bool wait = false;
    
    private void Start()
    {
        if (XPManager.ExitingLevel && !ResearchManager.AllResearchUnlocked())
        {
            IslandsLeftSetter.ReadyForButton += ReadyForButton;
            wait = true;
        }
    }

    private void ReadyForButton()
    {
        wait = false;
    }

    private void Update()
    {
        if (ResearchManager.CurrentResearch != null || ResearchManager.AllResearchUnlocked())
        {
            if (!wait)
            {
                NextLevelButton.SetActive(true);
            }
        } else
        {
            if (!wait)
            {
                NextLevelButton.SetActive(false);
            }
        }
    }
}
