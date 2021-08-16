using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetManager : MonoBehaviour
{
    [SerializeField] GameObject[] Resettables; 

    public void ResetButtonPressed()
    {
        foreach (GameObject g in Resettables)
        {
            g.GetComponent<IResettable>().ResetState();
        }

        print("If the build index changes this is going to break!");
        SceneManager.LoadScene(0);        
    }
}
