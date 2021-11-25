using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchSaveData
{
    public ResearchSaveData(int index)
    {
        Index = index;
        IslandsLeft = 0;
        Progress = 0;
        Unlocked = false;
    }

    public int Index;
    public int Progress;
    public int IslandsLeft;
    public bool Unlocked;
}
