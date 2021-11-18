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

    private bool RunTimer = false;
    float timer = 0f;

    float waveTime = 30f;

    private const int tutorialSeed = 958;

    bool firstLevel = true;

    //if this gets called then we're in the 'game' - we need to tell the xp manager that. 
    private void Awake()
    {
        ExperienceManager.SetExitingLevel(true);
        ExperienceManager.ResetXPMessages();
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
        //LoadingPanel.ShowLoadingPanel("Loading..."); //get a battle name at some point
        yield return null;
            
        if (MainMenuScript.TutorialMode)
        {
            print("Using tutorial seed!");
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

        StopSpawningWaves();

        UpdateCurrentWaveUI();
        UpdateNextWaveUI(CurrentLevelManager.GetFirstRoll(), CurrentLevelManager.GetMaxRoll());

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

    public void StopSpawningWaves()
    {
        RunTimer = false; 
    }

    public void StartNextWave()
    {
        if (!CurrentLevelManager.OnLastWave())
        {
            //RunTimer = true;
            CurrentLevelManager.SpawnNextWave();
            timer = 0f;
            SetWaveRollUI(CurrentLevelManager.GetPreviousSecondRoll());
            UpdateNextWaveUI(CurrentLevelManager.GetFirstRoll(), CurrentLevelManager.GetMaxRoll());
            UpdateCurrentWaveUI();
        }
    }

    private void Update()
    {
        if (RunTimer)
        {
            timer += Time.deltaTime;
            Slider.value = timer; 
            if (timer > waveTime)
            {
                StartNextWave();
            }
        }
    }

    public void SetWaveRollUI(int secondRoll)
    {
        RollResultText.Enable(string.Format("There will be an additional {0} enemies!", secondRoll));
    }

    public void UpdateCurrentWaveUI()
    {
        CurrentWaveText.text = string.Format("{0} / {1}", CurrentLevelManager.GetCurrentWave(), CurrentLevelManager.GetHighestWave());
    }
    
    public void UpdateNextWaveUI(int firstRoll, int maxRoll) //List<GameObject> NextWave
    {
        NextWaveText.text = string.Format("Scouts found: \n{0} enemies\nPlus between 1 and {1} more", firstRoll, maxRoll);

        /*
        Dictionary<string, int> enemyNameToNum = new Dictionary<string, int>();
        
        foreach (GameObject g in NextWave)
        {
            if (enemyNameToNum.ContainsKey(g.name))
            {
                enemyNameToNum[g.name] += 1;
            }
            else
            {
                enemyNameToNum[g.name] = 1;
            }
        }

        foreach (KeyValuePair<string, int> keyPair in enemyNameToNum)
        {
            NextWaveText.text += keyPair.Key + " x " + keyPair.Value + "\n";
        }
         */
    }

}
