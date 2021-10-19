using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    private static bool TutorialMode = true; //we'll have to load this from a file, or use PlayerPrefs. 
    private string tutorialModeKey = "TutorialMode";

    private void Awake()
    {
        TutorialMode = PlayerPrefs.GetInt(tutorialModeKey, 1) == 1 ? true : false;

        ShowPlayButtons();
    }

    public void PlayButtonPressed()
    {

    }

    private void ShowPlayButtons()
    {

    }
}