using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class XPApplier : MonoBehaviour
{
    //So, okay. If you unlock something I want to show it to you, then when you close it it'll resume. 
    //mm. That's fine. We can use flags to communicate with the coroutine. 

    [SerializeField] TextMeshProUGUI MessageText;
    [SerializeField] GameObject SkipButton;
    [SerializeField] GameObject NextLevelButton;
    [SerializeField] RewardPanelShower RewardPanelShower;

    private bool pause = false;
    private bool animate = true;

    public event Action GainedXP = delegate { };
    public event Action LeveledUp = delegate { };

    public event Action FinishedXPApply = delegate { };

    //so, as soon as this scene loads
    private void Awake()
    {
        if (ExperienceManager.ExitingLevel())
        {
            NextLevelButton.SetActive(false);
            SkipButton.SetActive(true);
            MessageText.gameObject.SetActive(true);

            ExperienceManager.SetExitingLevel(false);
            
            List<XPMessage> xpEarned = ExperienceManager.GetXPMessages();

            animate = true;
            pause = false;

            StartCoroutine(ShowXPGained(xpEarned));
        } else
        {
            MessageText.gameObject.SetActive(false);
            SkipButton.SetActive(false);
        }
    }

    private IEnumerator ShowXPGained(List<XPMessage> xpEarned)
    {
        float timer = 0f;

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

        ExperienceManager.WriteXP();
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

        bool gotReward = ExperienceManager.GainXP(message.XpGain);

        if (gotReward)
        {
            LeveledUp();

            pause = true; // I'm not a huge fan of this because it changes state sneakily. We should split this into two methods, one that applies the xp and returns if you got a reward
                        //and another that sets pause and shows the reward panel
            ShowRewardPanel(ExperienceManager.GetCurrentLevel()); //so, we'll just show what we got for the current level. 
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

    private void ShowRewardPanel(int level)
    {
        RewardPanelShower.ShowLevelRewards(level);
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
