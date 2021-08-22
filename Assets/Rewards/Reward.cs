using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward : MonoBehaviour
{
    [SerializeField] float quality;
    [SerializeField] bool reusable;
    [SerializeField] string description; 
    [SerializeField] Reward[] followingRewards; 

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

    public string Description
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
}