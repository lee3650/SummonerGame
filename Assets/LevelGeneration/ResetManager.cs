using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetManager : MonoBehaviour
{
    [SerializeField] GameObject[] Resettables;

    public void ResetButtonPressed()
    {
        ResetResettables();

        print("If the build index changes this is going to break!");
        SceneManager.LoadScene(1);
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

    public void QuitGame()
    {
        Application.Quit();
    }
}
