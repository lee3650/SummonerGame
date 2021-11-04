using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ProgressText;

    private TutorialManager TutorialManager;

    public void Initialize(TutorialManager tutorialManager)
    {
        TutorialManager = tutorialManager;
        ProgressText.text = tutorialManager.GetProgressText();
    }

    public void GoLeft()
    {
        TutorialManager.PrevPage();
    }

    public void GoRight()
    {
        TutorialManager.NextPage();
    }
}
