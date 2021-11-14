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

    private static int IslandsLeft = -1;

    private string IslandsLeftKey = "IslandsLeft";

    private void Awake()
    {
        if (IslandsLeft == -1)
        {
            IslandsLeft = PlayerPrefs.GetInt(IslandsLeftKey, CalculateIslandsLeft());
        }

        XPApplier.LeveledUp += LeveledUp;
        XPApplier.FinishedXPApply += FinishedXPApply;
        if (ExperienceManager.ExitingLevel())
        {
            NextLevelButton.SetActive(false);
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
        int amt = (int)(ExperienceManager.GetXPToNextLevel(ExperienceManager.GetCurrentLevel()) / (XPPerLevel + 3f * ExperienceManager.GetCurrentLevel())) + 1;

        IslandsLeftText.text = "Islands Left: " + amt;

        return amt; //we add 1 because some early levels could have 0 
    }

    private void FinishedXPApply()
    {
        if (IslandsLeft == 0)
        {
            //show a panel also. 
            PushedBackPanel.SetActive(true);
            StartCoroutine(ExperienceManager.AnimateXPToZero());
            IslandsLeft = CalculateIslandsLeft();
        }
        NextLevelButton.SetActive(true);
    }
}