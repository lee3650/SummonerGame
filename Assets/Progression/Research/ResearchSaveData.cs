using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchSaveData
{
    public ResearchSaveData()
    {
        //basically doesn't do anything, right? For XML serializer
    }

    public ResearchSaveData(int index)
    {
        Index = index;
        IslandsUsed = 0;
        Progress = 0;
        Unlocked = false;
    }

    public int Index;
    public float Progress;
    public int IslandsUsed;
    public bool Unlocked;
}
