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

    [SerializeField] int[] IslandsPerLevel;

    private static int IslandsLeft = -10;

    private const string IslandsLeftKey = "IslandsLeft";

    public static void WriteIslandsLeft()
    {
        PlayerPrefs.SetInt(IslandsLeftKey, IslandsLeft);
    }

    public static void ResetState()
    {
        IslandsLeft = -10;
        PlayerPrefs.DeleteKey(IslandsLeftKey);
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
        int curLevel = ExperienceManager.GetCurrentLevel();
        if (curLevel < IslandsPerLevel.Length)
        {
            return IslandsPerLevel[ExperienceManager.GetCurrentLevel()];
        }
        return 3;
    }

    private void Update()
    {
        IslandsLeftText.text = "Islands Left: " + IslandsLeft;
    }

    private void FinishedXPApply()
    {
        if (IslandsLeft <= 0)
        {
            PushedBackPanel.SetActive(true);
            StartCoroutine(ExperienceManager.AnimateXPToZero());
            IslandsLeft = CalculateIslandsLeft();
        }
        NextLevelButton.SetActive(true);
    }
}