using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NextWaveFunctionMonitor : MonoBehaviour
{
    [SerializeField] WaveSpawner WaveSpawner;
    [SerializeField] CurrentLevelManager CurrentLevelManager;
    [SerializeField] TextMeshProUGUI NextWaveButtonText; 

    void Update()
    {
        if (DoesNextWaveButtonStartNextLevel())
        {
            NextWaveButtonText.text = "Next Level";
        } else
        {
            NextWaveButtonText.text = "Start Next Wave";
        }
    }

    public bool DoesNextWaveButtonStartNextLevel()
    {
        return CurrentLevelManager.OnLastWave() && WaveSpawner.CurrentWaveDefeated();
    }
}
