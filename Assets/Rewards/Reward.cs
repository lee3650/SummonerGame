using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward : MonoBehaviour
{
    [SerializeField] float quality;
    [SerializeField] bool reusable;
    [SerializeField] string description; 
    [SerializeField] Reward[] followingRewards;
    public int UnlockedIndex;

    public abstract void ApplyReward();

    //quality is equal to relative probability - a reward with 10 quality is 10x more likely than a reward with 1 quality. 
    //lower quality is better. 
    public float Quality
    {
        get
        {
            return quality;
        }
    }

    public bool Reusable
    {
        get
        {
            return reusable;
        }
    }

    public Reward[] FollowingRewards
    {
        get
        {
            return followingRewards;
        }
    }

    public virtual string Description
    {
        get
        {
            return description;
        }
    }

    public float MinScore
    {
        get;
        set;
    }

    public float MaxScore
    {
        get;
        set;
    }

    public static char GetExternalQuality(float internalQuality)
    {
        if (internalQuality < 2)
        {
            return 'S';
        }
        if (internalQuality < 3)
        {
            return 'A';
        }
        if (internalQuality < 4)
        {
            return 'B';
        }
        if (internalQuality < 5)
        {
            return 'C';
        }
        return 'D';
    }
}