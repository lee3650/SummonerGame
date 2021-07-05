using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] PlayerEntity PlayerEntity;
    [SerializeField] List<GameObject> Enemies;
    [SerializeField] TextMeshProUGUI WaveText;
    [SerializeField] Transform TopRight, BottomLeft; //this is of the spawn zone. 

    private int waveNum = 1;

    float WaveTime = 30f;
    float timer = 0f;

    bool runTimer = false; 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SpawnEnemies(waveNum);    
            waveNum++;
            runTimer = true;
            timer = 0f;
        }

        if (runTimer)
        {
            timer += Time.deltaTime;
            if (timer > WaveTime)
            {
                timer = 0f;
                SpawnEnemies(waveNum);
                waveNum++; 
            }
        }
    }

    void SpawnEnemies(int num)
    {
        WaveText.text = "Wave: " + waveNum;

        for (int i = 0; i < num; i++)
        {
            GameObject enemy = Instantiate(Enemies[Random.Range(0, Enemies.Count)], GetRandomPointInSpawnZone(), Quaternion.Euler(Vector3.zero));
            enemy.GetComponent<AIEntity>().WakeUp();
            enemy.GetComponent<TargetManager>().Target = PlayerEntity;
        }
    }
    
    Vector2 GetRandomPointInSpawnZone()
    {
        return new Vector2(Random.Range(BottomLeft.position.x, TopRight.position.x), Random.Range(BottomLeft.position.y, TopRight.position.y)); 
    }
}
