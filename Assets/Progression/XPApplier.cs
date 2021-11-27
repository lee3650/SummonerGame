using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class XPApplier : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI MessageText;
    [SerializeField] GameObject SkipButton;
    [SerializeField] GameObject NextLevelButton;
    [SerializeField] ResearchPanel ResearchPanel;
    [SerializeField] GameObject HomeButton;
    [SerializeField] ResearchManager ResearchManager;

    private bool pause = false;
    private bool animate = true;

    public event Action GainedXP = delegate { };
    public event Action LeveledUp = delegate { };

    public event Action FinishedXPApply = delegate { };

    //so, as soon as this scene loads
    private void Start()
    {
        if (XPManager.ExitingLevel && !ResearchManager.AllResearchUnlocked())
        {
            NextLevelButton.SetActive(false);
            SkipButton.SetActive(true);
            MessageText.gameObject.SetActive(true);

            XPManager.ExitingLevel = false;

            List<XPMessage> xpEarned = XPManager.GetXPMessages();

            HomeButton.SetActive(false);

            animate = true;
            pause = false;

            StartCoroutine(ShowXPGained(xpEarned));
        } else
        {
            MessageText.gameObject.SetActive(false);
            SkipButton.SetActive(false);
        }
    }

    public void ExitToMainMenu()
    {
        LoadScript.LoadTo(Scenes.MainMenu, "Have a nice day!");
    }

    private IEnumerator ShowXPGained(List<XPMessage> xpEarned)
    {
        print("Showing xp gained!");
        float timer = 0f;

        while (ResearchManager.CurrentResearch == null && !ResearchManager.AllResearchUnlocked())
        {
            print("waiting for research!");
            yield return null; 
        }

        while (animate) 
        { 
            while (pause) 
            {
                yield return null;
            }

            timer += Time.deltaTime;

            if (xpEarned.Count == 0)
            {
                animate = false;
            }

            if (timer > getWaitTime(xpEarned))
            {
                timer = 0f;
                if (xpEarned.Count == 0)
                {
                    animate = false;
                } 
                else
                {
                
                    XPMessage cur = xpEarned[0];
                    xpEarned.RemoveAt(0);
                    ApplyXPMessage(cur);
                
                }
            }

            yield return null;
        }

        print("hiding skip button!");

        MessageText.gameObject.SetActive(false);
        ApplyAllRemainingMessage(xpEarned);
        SkipButton.SetActive(false);

        FinishedXPApply();

        NextLevelButton.SetActive(true);
        HomeButton.SetActive(true);
        ResearchManager.SaveResearchData();
    }

    private void ApplyAllRemainingMessage(List<XPMessage> xpEarned)
    {
        foreach (XPMessage m in xpEarned)
        {
            ApplyXPMessage(m);
        }
        pause = false;
    }

    private void ApplyXPMessage(XPMessage message)
    {
        SetUI(message);

        GainedXP();

        Research reward = ResearchManager.GainXP(message.XpGain);

        if (reward != null)
        {
            LeveledUp();

            pause = true; // I'm not a huge fan of this because it changes state sneakily. We should split this into two methods, one that applies the xp and returns if you got a reward
                        //and another that sets pause and shows the reward panel
            ShowResearchPanel(reward); //so, we'll just show what we got for the current level. 
        }
    }

    public void AllRewardPanelsClosed()
    {
        pause = false;
    }

    private void SetUI(XPMessage message)
    {
        MessageText.text = message.Message;
    }

    private void ShowResearchPanel(Research reward)
    {
        ResearchPanel.Show(reward, true);
    }

    private float getWaitTime(List<XPMessage> xpEarned)
    {
        return 1f / (2 * xpEarned.Count);
    }

    public void Skip()
    {
        animate = false;
    }

    public void PauseAnimation()
    {
        pause = true; 
    }

    public void PlayAnimation()
    {
        pause = false;
    }
}
