using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MainMenuScript : MonoBehaviour
{
    //set this to false if, in the setup scene, we want to not play the tutorial
    private static bool tutorialMode = false; //we'll have to load this from a file, or use PlayerPrefs. 
    private static string finishedTutorialKey = "TutorialMode";
    private bool FinishedTutorial;

    public const string appendPath = "data/";
    private const string tutorialFile = "finishedTutorial.txt";

    [SerializeField] Button PlayButton;
    [SerializeField] GameObject TutorialPlayButton;

    private void Awake()
    {
        FinishedTutorial = LoadFinishedTutorial();
        ShowPlayButtons();
    }

    private bool LoadFinishedTutorial()
    {
        bool finished = false;

        if (PlayerPrefs.GetInt(finishedTutorialKey, 0) == 1 ? true : false)
        {
            finished = true; 
        }

        if (File.Exists(Application.persistentDataPath + tutorialFile))
        {
            finished = true;
        } else
        {
            if (finished)
            {
                WriteTutorialFile();
            }
        }

        return finished;
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
        FinishedTutorial = false; 
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
            PlayButton.interactable = true;
        }
        else
        {
            PlayButton.interactable = false;
        }
    }

    public static void TutorialFinished()
    {
        print("Finished tutorial!");
        tutorialMode = false;
        PlayerPrefs.SetInt(finishedTutorialKey, 1);
        WriteTutorialFile();
    }

    private static void WriteTutorialFile()
    {
        File.WriteAllText(Application.persistentDataPath + tutorialFile, "");
    }

    public static bool TutorialMode
    {
        get
        {
            return tutorialMode;
        }
    }
}