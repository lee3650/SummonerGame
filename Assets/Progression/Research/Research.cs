using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Research : MonoBehaviour
{
    [SerializeField] private int index;
    public VideoClip Gif;
    [HideInInspector] public Sprite Image;
    public ResearchSaveData ResearchSaveData = null;
    public int TotalIslands;
    public string Description; //eh, that's fine for now, I guess. I'd rather read it from a file. 
    public Research Prereq = null;
    public float XPReq;
    public GameObject[] Unlocks;

    public bool PrereqUnlocked
    {
        get
        {
            if (Prereq == null)
            {
                return true;
            }
            return Prereq.Unlocked;
        }
    }

    public int Index
    {
        get
        {
            return index;
        }
    }

    public bool Unlocked
    {
        get
        {
            return ResearchSaveData.Unlocked;
        }
        set
        {
            ResearchSaveData.Unlocked = value;
        }
    }

    public float Progress
    {
        get
        {
            return ResearchSaveData.Progress;
        }
        set
        {
            ResearchSaveData.Progress = value;
        }
    }

    public int IslandsLeft
    {
        get
        {
            return TotalIslands - ResearchSaveData.IslandsUsed;
        }
        set
        {
            ResearchSaveData.IslandsUsed = TotalIslands - value;
        }
    }
}
