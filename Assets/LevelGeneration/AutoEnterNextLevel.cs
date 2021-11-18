using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoEnterNextLevel : MonoBehaviour
{
    [SerializeField] LoadingPanel LoadingPanel;
    [SerializeField] WaveViewModel WaveViewModel;

    private void Awake()
    {
        if (MainMenuScript.TutorialMode)
        {
            LoadingPanel.ShowLoadingPanel("Training Camp"); //I do want to, um, generate names at some point. 
        }
        else
        {                                                   //why don't we just leave this in ram? 
            LoadingPanel.ShowLoadingPanel("The Battle of " + IslandNamer.NextIslandName + " Island"); //I do want to, um, generate names at some point. 
        }

        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;

        WaveViewModel.EnterNextLevel();
    }

}
