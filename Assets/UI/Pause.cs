using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject PauseButton;

    public void PauseGame()
    {
        Time.timeScale = 0f;

        gameObject.SetActive(true);
        PauseButton.SetActive(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        
        gameObject.SetActive(false);
        PauseButton.SetActive(true);
    }
}
