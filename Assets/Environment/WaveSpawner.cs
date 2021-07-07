using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] PlayerEntity PlayerEntity;
    [SerializeField] List<GameObject> Enemies;
    [SerializeField] Slider Slider;
    [SerializeField] TextMeshProUGUI WaveText;
    [SerializeField] TextMeshProUGUI NextWaveText;
    [SerializeField] Transform TopRight, BottomLeft; //this is of the spawn zone. 

    List<GameObject> NextWave = new List<GameObject>(); 

    private int waveNum = 1;

    float WaveTime = 30f;
    float timer = 0f;

    bool runTimer = false;

    private void Awake()
    {
        GenerateNextWave(1);
    }

    private void Update()
    {
        WaveText.text = "Wave: " + waveNum;

        if (runTimer)
        {
            timer += Time.deltaTime;
            if (timer > WaveTime)
            {
                SpawnAndGenerateWave();
            }
        }

        Slider.value = timer; 
    }

    void GenerateNextWave(int num)
    {
        NextWave = new List<GameObject>();

        for (int i = 0; i < num; i++)
        {
            NextWave.Add(Enemies[Random.Range(0, Enemies.Count)]);
        }

        UpdateNextWaveUI();
    }

    public void SpawnAndGenerateWave()
    {
        timer = 0f;
        SpawnNextWave();
        waveNum++;
        GenerateNextWave(waveNum);
        runTimer = true;
    }

    void UpdateNextWaveUI()
    {
        NextWaveText.text = "";

        Dictionary<string, int> enemyNameToNum = new Dictionary<string, int>();

        foreach (GameObject g in NextWave)
        {
            if (enemyNameToNum.ContainsKey(g.name))
            {
                enemyNameToNum[g.name] += 1; 

            } else
            {
                enemyNameToNum[g.name] = 1;
            }
        }

        foreach (KeyValuePair<string, int> keyPair in enemyNameToNum)
        {
            NextWaveText.text += keyPair.Key + " x " + keyPair.Value + "\n";
        }
    }
    
    void SpawnNextWave()
    {
        for (int i = 0; i < NextWave.Count; i++)
        {
            GameObject enemy = Instantiate(NextWave[i], GetRandomPointInSpawnZone(), Quaternion.Euler(Vector3.zero));
            enemy.GetComponent<AIEntity>().WakeUp();
            enemy.GetComponent<TargetManager>().Target = PlayerEntity;
        }
    }

    Vector2 GetRandomPointInSpawnZone()
    {
        return new Vector2(Random.Range(BottomLeft.position.x, TopRight.position.x), Random.Range(BottomLeft.position.y, TopRight.position.y)); 
    }
}
