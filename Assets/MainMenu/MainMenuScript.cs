using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    //set this to false if, in the setup scene, we want to not play the tutorial
    private static bool tutorialMode = false; //we'll have to load this from a file, or use PlayerPrefs. 
    private static string finishedTutorialKey = "TutorialMode";
    private bool FinishedTutorial;

    public const string appendPath = "data/";

    [SerializeField] GameObject PlayButton;
    [SerializeField] GameObject TutorialPlayButton;

    private void Awake()
    {
        FinishedTutorial = PlayerPrefs.GetInt(finishedTutorialKey, 0) == 1 ? true : false;
        ShowPlayButtons();
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetState()
    {
        XPManager.ResetState();
        ResearchManager.ResetState();
        LetterManager.ResetState();
        tutorialMode = false;
        ShowPlayButtons();
    }

    public void PlayButtonPressed()
    {
        tutorialMode = false;
        LoadScript.LoadTo(Scenes.ProgressionMenu, "Loading...");
    }

    public void PlayTutorialButtonPressed()
    {
        tutorialMode = true;
        LoadScript.LoadTo(Scenes.GameplayScene, "Fifteen Years Ago");
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