using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResetLevel : MonoBehaviour
{
    [SerializeField] XPApplier XPApplier;
    [SerializeField] GameObject NextLevelButton;
    [SerializeField] TextMeshProUGUI IslandsLeftText;
    [SerializeField] GameObject PushedBackPanel;

    private float XPPerLevel = 15f;

    private static int IslandsLeft = -10;

    private const string IslandsLeftKey = "IslandsLeft";

    public static void WriteIslandsLeft()
    {
        PlayerPrefs.SetInt(IslandsLeftKey, IslandsLeft);
    }

    private void Awake()
    {
        if (IslandsLeft == -10)
        {
            IslandsLeft = PlayerPrefs.GetInt(IslandsLeftKey, CalculateIslandsLeft()); //so, basically it'll only calculate it the first time and when you level up. 
        }

        XPApplier.LeveledUp += LeveledUp;
        XPApplier.FinishedXPApply += FinishedXPApply;
        if (ExperienceManager.ExitingLevel())
        {
            NextLevelButton.SetActive(false);
        } else
        {
            if (IslandsLeft < 0)
            {
                IslandsLeft = CalculateIslandsLeft();
            }
        }

        IslandsLeftText.text = "Islands Left: " + IslandsLeft;
    }

    public void ReduceIslandsLeft()
    {
        IslandsLeft--;
        PlayerPrefs.SetInt(IslandsLeftKey, IslandsLeft);
    }

    private void LeveledUp()
    {
        IslandsLeft = CalculateIslandsLeft();
    }

    private int CalculateIslandsLeft()
    {
        int amt = (int)(ExperienceManager.GetXPToNextLevel(ExperienceManager.GetCurrentLevel()) / (XPPerLevel + 3f * ExperienceManager.GetCurrentLevel())) + 1;

        IslandsLeftText.text = "Islands Left: " + amt;

        return amt; //we add 1 because some early levels could have 0... at some point we'll just have to define it. 
    }

    private void FinishedXPApply()
    {
        if (IslandsLeft <= 0)
        {
            PushedBackPanel.SetActive(true);
            StartCoroutine(ExperienceManager.AnimateXPToZero());
            IslandsLeft = CalculateIslandsLeft();
            IslandsLeftText.text = "Islands Left: " + IslandsLeft;
        }
        NextLevelButton.SetActive(true);
    }
}