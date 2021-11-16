using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayRewardData : MonoBehaviour
{
    public RewardPanelShower RewardPanelShower;
    public bool HasGif;
    public ImageSequence Gif;
    [Tooltip("This is set at awake by the progression unlock that has a reference to this drd")]
    public bool IsItem;

    public string TextPath;
    public Sprite OverideBackground;
}
