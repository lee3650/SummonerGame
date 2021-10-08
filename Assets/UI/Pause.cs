using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject PauseButton;
    float defaultDeltaTime;

    private void Awake()
    {
        defaultDeltaTime = Time.fixedDeltaTime;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;

        gameObject.SetActive(true);
        PauseButton.SetActive(false);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = defaultDeltaTime;
        gameObject.SetActive(false);
        PauseButton.SetActive(true);
    }
}
