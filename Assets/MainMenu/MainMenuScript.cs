﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    private static bool tutorialMode = true; //we'll have to load this from a file, or use PlayerPrefs. 
    private static string finishedTutorialKey = "TutorialMode";
    private bool FinishedTutorial;

    [SerializeField] GameObject PlayButton;
    [SerializeField] GameObject TutorialPlayButton;
    [SerializeField] string mainSceneName = "SetupScene";

    private void Awake()
    {
        FinishedTutorial = PlayerPrefs.GetInt(finishedTutorialKey, 0) == 1 ? true : false;
        ShowPlayButtons();
    }

    public void PlayButtonPressed()
    {
        tutorialMode = false; 
        SceneManager.LoadScene(mainSceneName);
    }

    public void PlayTutorialButtonPressed()
    {
        tutorialMode = true;
        SceneManager.LoadScene(mainSceneName);
    }

    private void ShowPlayButtons()
    {
        if (FinishedTutorial)
        {
            //show both the tutorial button and the play button
            PlayButton.SetActive(true);
            TutorialPlayButton.SetActive(true);
        }
        else
        {
            //show just the tutorial button
            TutorialPlayButton.SetActive(true);
            PlayButton.SetActive(false);
        }
    }

    public static void TutorialFinished()
    {
        tutorialMode = false;
        PlayerPrefs.SetInt(finishedTutorialKey, 1);
    }

    public static bool TutorialMode
    {
        get
        {
            return tutorialMode;
        }
    }
}