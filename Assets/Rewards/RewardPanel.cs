using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RewardPanel : MonoBehaviour
{
    [SerializeField] OpenButton OpenButton; 
    [SerializeField] TextMeshProUGUI RollDescriptionText;

    [SerializeField] TextMeshProUGUI RewardName;
    [SerializeField] TextMeshProUGUI QualityText;
    [SerializeField] TextMeshProUGUI Description;
    [SerializeField] GameObject CloseButton;
    [SerializeField] RewardViewModel RewardViewModel; //I'm definitely not doing this right 
    [SerializeField] TextMeshProUGUI ChestChanceInfo;
    [SerializeField] GameObject DescInformation;

    public void Show(float odds)
    {
        OpenButton.Interactable = true;

        DescInformation.SetActive(false);

        CloseButton.SetActive(false);

        ChestChanceInfo.text = string.Format("({0}% chance)", odds);

        gameObject.SetActive(true);
    }

    public void OpenButtonPressed()
    {
        DescInformation.SetActive(true);
        OpenButton.Interactable = false;
        RewardViewModel.WonSpin();
        CloseButton.gameObject.SetActive(true);
    }

    //this is weird now because it kind of comes out of nowhere. This should really just be one script with the viewmodel and this 
    public void WonReward(Reward reward)
    {
        RewardName.text = reward.name;

        Description.text = reward.Description;

        QualityText.text = string.Format("Quality: {0}", Reward.GetExternalQuality(reward.Quality));
    }

    private void Update()
    {
        if (CloseButton.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape)) 
        {
            Close();
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
