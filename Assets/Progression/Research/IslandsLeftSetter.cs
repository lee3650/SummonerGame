using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class IslandsLeftSetter : MonoBehaviour
{
    [SerializeField] XPApplier XPApplier;
    [SerializeField] GameObject PushedBackPanel;
    [SerializeField] ResearchManager ResearchManager;
    [SerializeField] GameObject NextLevelButton;

    public event Action ReadyForButton = delegate { };

    private void Awake()
    {
        XPApplier.FinishedXPApply += FinishedXPApply;
    }

    private void FinishedXPApply()
    {
        ReadyForButton();

        /*
        if (ResearchManager.CurrentResearch != null)
        {
            if (ResearchManager.CurrentResearch.IslandsLeft <= 0 && !ResearchManager.CurrentResearch.Unlocked)
            {
                //so, if we finished our apply and there's still 0 left...
                //and it's not unlocked, I guess, then we failed. 

                PushedBackPanel.SetActive(true);
                NextLevelButton.SetActive(false);
                StartCoroutine(AnimateXPToZero());
            } else
            {
                ReadyForButton();
            }
        }
         */
    }

    IEnumerator AnimateXPToZero()
    {
        ResearchManager.Interactable = false;

        float its = 20;
        float start = ResearchManager.CurrentResearch.Progress;

        for (int i = 0; i < its; i++)
        {
            ResearchManager.CurrentResearch.Progress = Mathf.Lerp(start, 0f, i / its);
            yield return null;
        }

        ResearchManager.CurrentResearch.Progress = 0f;
        ResearchManager.CurrentResearch.IslandsLeft = ResearchManager.CurrentResearch.TotalIslands;

        ResearchManager.Interactable = true;

        NextLevelButton.SetActive(true);
        //PushedBackPanel.SetActive(false);

        ResearchManager.WriteResearchSaveDatas();

        ReadyForButton();
    }

    public void DecrementIslandsLeft()
    {
        if (ResearchManager.CurrentResearch != null)
        {
            ResearchManager.CurrentResearch.IslandsLeft -= 1;
        }
    }
}
