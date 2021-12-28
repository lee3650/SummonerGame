using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//I'm not really sure where to put that in the file system. 
public class WaveViewModel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI NextWaveText;
    [SerializeField] EnableForTime RollResultText;
    [SerializeField] TextMeshProUGUI CurrentWaveText;
    [SerializeField] TextMeshProUGUI LevelNum;
    [SerializeField] GameObject NextWaveButton;
    [SerializeField] GameObject NextLevelButton;

    [SerializeField] WarshipManager WarshipManager;
    [SerializeField] WaveSpawner WaveSpawner;
    [SerializeField] NextLevelEvent NextLevelEvent;

    [SerializeField] Summoner PlayerSummoner;

    [SerializeField] Slider Slider;

    [SerializeField] CurrentLevelManager CurrentLevelManager;

    [SerializeField] NextWaveFunctionMonitor NextWaveFunctionMonitor;

    [SerializeField] LoadingPanel LoadingPanel;

    private const int tutorialSeed = 1911;

    bool firstLevel = true;

    //if this gets called then we're in the 'game' - we need to tell the xp manager that. 
    private void Awake()
    {
        XPManager.ExitingLevel = true;
        XPManager.ResetXPMessages();
    }

    //yeah I'm definitely making this too complicated. 
    public void EnterNextLevel()
    {
        if (firstLevel)
        {
            StartCoroutine(SetupFirstLevel());
        } else
        {
            SetupRegularLevel();
        }
    }

    private IEnumerator SetupFirstLevel()
    {
        yield return null;
        
        if (MainMenuScript.TutorialMode)
        {
            print("Using tutorial seed: " + tutorialSeed);
            Random.InitState(tutorialSeed);
        }
        else
        {
            int seed = System.DateTime.Now.Millisecond * System.DateTime.Now.Second;
            print("seed: " + seed);
            Random.InitState(seed);
        }

        firstLevel = false;
        CurrentLevelManager.EnterFirstLevel();
        NextWaveButton.SetActive(false);

        SetupRegularLevel();

        LoadingPanel.HideLoadingPanel();
    }

    private void SetupRegularLevel()
    {
        CurrentLevelManager.EnterNextLevel();

        UpdateCurrentWaveUI();
        UpdateNextWaveUI(CurrentLevelManager.GetNextWaveDescription());

        NextLevelButton.SetActive(false);

        LevelNum.text = "Level: " + CurrentLevelManager.GetLevelNum();

        WaveSpawner.MakeRandomPointsAvailable();
        WarshipManager.SpawnShips();

        //MinerSummon.ScaleCost(PlayerSummoner);

        NextLevelEvent.TriggerOnNextLevelEvent();
    }

    public void SpawnNextWaveButtonPressed()
    {
        StartNextWave();
        NextWaveButton.SetActive(false);
    }

    public void StartNextWave()
    {
        if (!CurrentLevelManager.OnLastWave())
        {
            CurrentLevelManager.SpawnNextWave();

            UpdateNextWaveUI(CurrentLevelManager.GetNextWaveDescription());
            SetWaveRollUI(CurrentLevelManager.GetCurrentWaveDescription());

            UpdateCurrentWaveUI();
        }
    }

    public void SetWaveRollUI(string text)
    {
        RollResultText.Enable(text);
    }

    public void UpdateCurrentWaveUI()
    {
        CurrentWaveText.text = string.Format("{0} / {1}", CurrentLevelManager.GetCurrentWave(), CurrentLevelManager.GetHighestWave());
    }
    
    public void UpdateNextWaveUI(string text)
    {
        NextWaveText.text = text;
    }
}
