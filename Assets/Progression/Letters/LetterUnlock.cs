using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LetterUnlock
{
    public LetterType Type;
    public GameplayChange GameplayChange;
    public List<SpawnToProbability> EnemiesAdded;
    public int TotalIslandsReq;
    public string path;
    public string Date; 
}
