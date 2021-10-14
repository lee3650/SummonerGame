using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject PauseButton;

    private bool paused = false;

    public void TogglePauseGame()
    {
        if (paused)
        {
            ResumeGame();
            paused = false;
        } else
        {
            PauseGame();
            paused = true;
        }
    }

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
