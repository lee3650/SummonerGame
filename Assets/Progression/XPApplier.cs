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

            //so, the issue with this is that it can only do at most 1 xp earned per frame, right. 
            //Which means that if you have 1000s of xp messages, it's going to 
            //only apply 1 per frame, so it's going to take 1000s of frames to finish
            //so... what we want to is 
            //divide the elapsed time 
            //by the length of the xp messages
            //then, um, apply that many
            //basically. I think it makes sense. 

            if (timer > getWaitTime(xpEarned))
            {
                int numToApply = 1;
                if (getWaitTime(xpEarned) < Time.deltaTime)
                {
                    numToApply = CalcNumToApply(xpEarned.Count, timer);
                }

                timer = 0f;

                for (int i = 0; i < numToApply; i++)
                {
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

    private int CalcNumToApply(int count, float timer)
    {
        float e = 2.718281828459f;
        int x = Mathf.RoundToInt(Mathf.Pow(e, Mathf.Log(count) - (2 * timer)));
        return count - x;
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

    //note that CalcNumToApply duplicates this 
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
