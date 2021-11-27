using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResearchOption : MonoBehaviour, IPointerClickHandler
{
    //the only sketchy thing here is that this will have to know about it's current level and constantly show it... 
    [SerializeField] private int RewardIndex;
    private ResearchManager ResearchManager;

    [SerializeField] GameObject LockIcon;
    [SerializeField] Slider Slider;

    public void Init(ResearchManager rm)
    {
        ResearchManager = rm;
    }    

    public void OnPointerClick(PointerEventData data)
    {
        ResearchManager.ShowResearchPanel(RewardIndex);
    }

    private void Update()
    {
        if (ResearchManager != null)
        {
            if (ResearchManager.ResearchUnlocked(RewardIndex))
            {
                LockIcon.SetActive(false);
                Slider.gameObject.SetActive(false);
            }
            else
            {
                LockIcon.SetActive(true);

                if (ResearchManager.GetResearchPercent(RewardIndex) > 0)
                {
                    Slider.gameObject.SetActive(true);
                    Slider.value = ResearchManager.GetResearchPercent(RewardIndex);
                }
            }
        }    
    }
}
