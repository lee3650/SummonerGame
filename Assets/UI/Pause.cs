using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject PauseButton;

    [SerializeField] AudioSource OceanSource;
    [SerializeField] MusicManager MusicManager;

    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Slider OceanSlider;

    [SerializeField] SetVolumes SetVolumes;

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

        SetVolumes.SetupSliders();

        MusicManager.Pause();
        OceanSource.Pause();

        gameObject.SetActive(true);
        PauseButton.SetActive(false);
    }

    public void ResumeGame()
    {
        SharedSoundManager.WriteVolumes();
        Time.timeScale = 1f;

        MusicManager.Play();
        OceanSource.Play();

        gameObject.SetActive(false);
        PauseButton.SetActive(true);
    }
}
