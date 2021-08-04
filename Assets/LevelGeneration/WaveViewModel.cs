using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//I'm not really sure where to put that in the file system. 
public class WaveViewModel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI NextWaveText;
    [SerializeField] TextMeshProUGUI CurrentWaveText;
    [SerializeField] TextMeshProUGUI LevelNum; 

    [SerializeField] Summoner PlayerSummoner;

    [SerializeField] Slider Slider;

    [SerializeField] CurrentLevelManager CurrentLevelManager;

    [SerializeField] NextWaveFunctionMonitor NextWaveFunctionMonitor;

    private bool RunTimer = false;
    float timer = 0f;

    float waveTime = 30f;

    bool firstLevel = true; 

    //yeah I'm definitely making this too complicated. 

    public void EnterNextLevelOrSpawnWave()
    {
        if (firstLevel)
        {
            firstLevel = false;
            CurrentLevelManager.EnterFirstLevel();
        }

        //this is where we give out our new items and charms and such 
        if (NextWaveFunctionMonitor.DoesNextWaveButtonStartNextLevel())
        {
            CurrentLevelManager.EnterNextLevel();

            StopSpawningWaves();

            UpdateCurrentWaveUI();
            UpdateNextWaveUI(CurrentLevelManager.GetNextWave());

            LevelNum.text = "Level: " + CurrentLevelManager.GetLevelNum();
        }
        else
        {
            StartNextWave();
        }
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
            UpdateNextWaveUI(CurrentLevelManager.GetNextWave());
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

    public void UpdateCurrentWaveUI()
    {
        CurrentWaveText.text = string.Format("{0} / {1}", CurrentLevelManager.GetCurrentWave(), CurrentLevelManager.GetHighestWave());
    }

    public void UpdateNextWaveUI(List<GameObject> NextWave)
    {
        NextWaveText.text = "";

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
    }

}
