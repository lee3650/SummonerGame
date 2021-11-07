using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject PauseButton;

    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource OceanSource;

    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Slider OceanSlider;

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

        SetupSliders();
       
        MusicSource.Pause();
        OceanSource.Pause();

        gameObject.SetActive(true);
        PauseButton.SetActive(false);
    }

    private void SetupSliders()
    {
        float music = SharedSoundManager.GetMusicVolumeLevel();
        float sfx = SharedSoundManager.GetSFXVolumeLevel();
        float ocean = SharedSoundManager.GetOceanVolume();

        MusicSlider.value = music;
        SFXSlider.value = sfx;
        OceanSlider.value = ocean;
    }

    public void ResumeGame()
    {
        SharedSoundManager.WriteVolumes();
        Time.timeScale = 1f;

        MusicSource.Play();
        OceanSource.Play();

        gameObject.SetActive(false);
        PauseButton.SetActive(true);
    }
}
