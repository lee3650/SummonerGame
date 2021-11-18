using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoEnterNextLevel : MonoBehaviour
{
    [SerializeField] LoadingPanel LoadingPanel;
    [SerializeField] WaveViewModel WaveViewModel;

    private string[] names = new string[]
    {
        "Gaptig",
        "Camins",
        "Helion",
        "Erlend",
        "Dharaz",
        "Yipire",
        "Huchne",
        "Brelog",
        "Buotro",
        "Pyxeli"
    };

    private void Awake()
    {
        if (MainMenuScript.TutorialMode)
        {
            LoadingPanel.ShowLoadingPanel("Training Camp"); //I do want to, um, generate names at some point. 
        }
        else
        {
            LoadingPanel.ShowLoadingPanel("The Battle of " + names[Random.Range(0, names.Length)] + " Island"); //I do want to, um, generate names at some point. 
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
