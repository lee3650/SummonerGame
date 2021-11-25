using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResearchOption : MonoBehaviour, IPointerClickHandler
{
    //the only sketchy thing here is that this will have to know about it's current level and constantly show it... 
    [SerializeField] private int RewardIndex;

    private ResearchManager ResearchManager;

    public void Init(ResearchManager rm)
    {
        ResearchManager = rm;
    }    

    public void OnPointerClick(PointerEventData data)
    {
        ResearchManager.ShowResearchPanel(RewardIndex);
    }
}
