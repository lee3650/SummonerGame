using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndPanel : MonoBehaviour, IWaveNotifier
{
    [SerializeField] HealthManager PlayerHealth;
    [SerializeField] GameObject LosePanel;
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject NextWaveButton;
    [SerializeField] GameObject NextLevelButton;
    [SerializeField] CurrentLevelManager CurrentLevelManager;

    private const string ProgressionSceneName = "ProgressionScene";

    private void Awake()
    {
        PlayerHealth.OnDeath += OnDeath;
        WaveSpawner.NotifyWhenWaveEnds(this);
    }

    private void OnDeath()
    {
        HideButtons();
        LosePanel.SetActive(true);
    }
    
    //how do we know if the game was won? 
    //I guess ask current level manager at the end of every wave. 

    public void OnWaveEnds()
    {
        if (CurrentLevelManager.OnFinalWaveOfGame())
        {
            HideButtons();
            WinPanel.SetActive(true);
        }
    }

    private void HideButtons()
    {
        NextLevelButton.SetActive(false);
        NextWaveButton.SetActive(false);
    }

    public void Continue()
    {
        SceneManager.LoadScene(ProgressionSceneName);
    }
}
