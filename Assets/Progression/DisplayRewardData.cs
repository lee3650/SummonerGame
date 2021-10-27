using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayRewardData : MonoBehaviour
{
    public RewardPanelShower RewardPanelShower;
    public bool HasGif;
    public ImageSequence Gif;
    //so, we need a path to text, right. 
    //Either way. 
    //And we also need to set the background, depending on if it's a letter or if it's an item. 
    public string TextPath;
    public Sprite OverideBackground;
}
