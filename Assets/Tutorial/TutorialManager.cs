using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] StringDisplayPanel TutorialDisplayPanel;
    [SerializeField] PanelDisplayer TutorialPanelDisplayer;
    [SerializeField] NextLevelEvent NextLevelEvent;


    const string tutorialFileName = "ttl";

    string[][] tutorialText;
    int levelCounter = 0;

    Vector2Int SectionAndSegment;

    private bool changeText = false;

    private void Start()
    {
        if (MainMenuScript.TutorialMode)
        {
            //convention: use \n to separate each section and use / to separate parts within a section - a segment
            string fileContents = File.ReadAllText(tutorialFileName);
            tutorialText = ParseFileContents(fileContents);
            NextLevelEvent.OnNextLevel += OnNextLevel;
            SectionAndSegment = new Vector2Int();
            changeText = true;
        }
    }

    private void OnNextLevel()
    {
        // we can safely assume we're in tutorial mode
        levelCounter++;
        if (levelCounter == 1)
        {
            SectionAndSegment = new Vector2Int(1, 0);
            changeText = true;
        } else
        {
            //finish tutorial
            EndTutorial();
        }
    }

    private void EndTutorial()
    {
        //we'll have to kick you out somehow - change scenes, I guess. 
        //so, we have to get you to level 2, actually - you need to get your default stuff (level 1) and a letter (level 2). 
        //Then we'll be kind of 'setup' for the standard progression

        MainMenuScript.TutorialFinished();
        ExperienceManager.GainXP(ExperienceManager.FirstTwoLevelXP);
    }

    private void Update()
    {
        if (MainMenuScript.TutorialMode)
        {
            //I guess I use left click to increment the tutorial?
            if (changeText)
            {
                changeText = false;
                ShowTutorialText(SectionAndSegment);
            }

            if (Input.GetMouseButtonDown(0))
            {
                changeText = true;
                SectionAndSegment = incrementY(SectionAndSegment, tutorialText[SectionAndSegment.x].Length - 1); //okay. So, segment should be click based but section should be event based. 
            }
        }
    }

    private Vector2Int incrementY(Vector2Int start, int max)
    {
        start += new Vector2Int(0, 1);
        if (start.y > max)
        {
            start = new Vector2Int(start.x, 0);
        }
        return start;
    }

    private void ShowTutorialText(Vector2Int secAndSeg)
    {
        TutorialPanelDisplayer.HideAllPanels();
        TutorialPanelDisplayer.ShowPanel(TutorialDisplayPanel, tutorialText[secAndSeg.x][secAndSeg.y]);
    }

    private string[][] ParseFileContents(string text)
    {
        string[] sections = text.Split('\n');

        string[][] result = new string[sections.Length][];

        for (int i = 0; i < sections.Length; i++)
        {
            result[i] = sections[i].Split('/');
        }

        return result;
    }
}
