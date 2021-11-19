using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetManager : MonoBehaviour
{
    [SerializeField] GameObject[] Resettables;

    public void ExitButtonPressed()
    {
        ExitToScene(Scenes.ProgressionMenu, "Loading");
    }

    public void ExitToScene(Scenes scene, string message)
    {
        ResetResettables();
        LoadScript.LoadTo(scene, message);
    }

    private void ResetResettables()
    {
        foreach (GameObject g in Resettables)
        {
            g.GetComponent<IResettable>().ResetState();
        }
    }
}
