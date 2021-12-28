using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResearchOption : MonoBehaviour, IPointerClickHandler
{
    //the only sketchy thing here is that this will have to know about its current level and constantly show it... 
    private int RewardIndex;
    private ResearchManager ResearchManager;

    [SerializeField] Research MyResearch;
    [SerializeField] Image Image; 
    [SerializeField] GameObject LockIcon;
    [SerializeField] Slider Slider;
    [SerializeField] GameObject Blocker; 

    public void Init(ResearchManager rm)
    {
        ResearchManager = rm;
        RewardIndex = MyResearch.Index;
        MyResearch.Image = Image.sprite;
    }

    public void Hide()
    {
        Blocker.SetActive(true);
    }

    public void Show()
    {
        Blocker.SetActive(false);
    }

    public void OnPointerClick(PointerEventData data)
    {
        ResearchManager.ShowResearchPanel(RewardIndex);
    }

    private void Update()
    {
        if (MyResearch != null)
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
